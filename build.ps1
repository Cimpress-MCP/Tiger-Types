<#

.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.

.DESCRIPTION
This Powershell script will download NuGet if missing, restore NuGet tools (including Cake)
and execute your Cake build script with the parameters you provide.

.PARAMETER Script
The build script to execute.
.PARAMETER Target
The build script target to run.
.PARAMETER Configuration
The build configuration to use.
.PARAMETER Verbosity
Specifies the amount of information to be displayed.
.PARAMETER Experimental
Tells Cake to use the latest Roslyn release.
.PARAMETER WhatIf
Performs a dry run of the build script.
No tasks will be executed.
.PARAMETER Mono
Tells Cake to use the Mono scripting engine.
.PARAMETER SkipToolPackageRestore
Skips restoring of packages.
.PARAMETER ScriptArgs
Remaining arguments are added here.

.LINK
http://cakebuild.net

#>

[CmdletBinding()]
Param(
    [string]$Script = "build.cake",
    [string]$Target = "Default",
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity = "Quiet",
    [switch]$Experimental,
    [Alias("DryRun","Noop")]
    [switch]$WhatIf,
    [switch]$SkipToolPackageRestore,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

Write-Host "Preparing to run build script..."

if(!$PSScriptRoot){
    $PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}

# Should we show verbose messages?
if($Verbose.IsPresent)
{
    $VerbosePreference = "continue"
}

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$PACKAGES_CONFIG = Join-Path $TOOLS_DIR "Packages.config"

# Ensure build dependencies are installed.
Write-Verbose -Message "Restoring build dependencies."
Invoke-Expression "&choco install Bootstrap.config --confirm --limitoutput" | out-null

# Should we use the new Roslyn?
$UseExperimental = "";
if($Experimental.IsPresent -And !($Mono.IsPresent)) {
    Write-Verbose -Message "Using experimental version of Roslyn."
    $UseExperimental = "-experimental"
}

# Is this a dry run?
$UseDryRun = "";
if($WhatIf.IsPresent) {
    $UseDryRun = "-dryrun"
}

# Make sure tools folder exists
if ((Test-Path $PSScriptRoot) -And !(Test-Path $TOOLS_DIR)) {
    Write-Verbose -Message "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type directory | out-null
}

# Restore tools from NuGet?
if(-Not $SkipToolPackageRestore.IsPresent -Or !(Test-Path $PACKAGES_CONFIG))
{
    # Restore packages from NuGet.
    Push-Location
    Set-Location $TOOLS_DIR

    Write-Verbose -Message "Restoring tools from NuGet..."
    $NuGetOutput = Invoke-Expression "&nuget install -ExcludeVersion -OutputDirectory `"$TOOLS_DIR`""
    Write-Verbose -Message ($NuGetOutput | out-string)

    Pop-Location
    if ($LASTEXITCODE -ne 0)
    {
        exit $LASTEXITCODE
    }
}

# Start Cake
Write-Host "Running build script..."
Invoke-Expression "cake `"$Script`" -target=`"$Target`" -configuration=`"$Configuration`" -verbosity=`"$Verbosity`" $UseDryRun $UseExperimental $ScriptArgs"
exit $LASTEXITCODE
