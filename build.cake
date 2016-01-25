// Arguments

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosity = Argument("verbosity", Verbosity.Quiet);

var nuGetVerbosities = new Dictionary<Verbosity, NuGetVerbosity>
{
  { Verbosity.Quiet, NuGetVerbosity.Quiet },
  { Verbosity.Minimal, NuGetVerbosity.Normal },
  { Verbosity.Normal, NuGetVerbosity.Normal },
  { Verbosity.Verbose, NuGetVerbosity.Detailed },
  { Verbosity.Diagnostic, NuGetVerbosity.Detailed }
};
// note(cosborn) NuGet has to go and be different.  Well, fine.
var nuGetVerbosity = nuGetVerbosities[verbosity];

var nUnitVerbosities = new Dictionary<Verbosity, bool>
{
  { Verbosity.Quiet, false },
  { Verbosity.Minimal, false },
  { Verbosity.Normal, false },
  { Verbosity.Verbose, true },
  { Verbosity.Diagnostic, true }
};
// note(cosborn) NUnit uses only a -Verbose switch.
var nUnitVerbosity = nUnitVerbosities[verbosity];

// Global Variables

// anti-convention: Solution files can be anywhere.
var solutions = GetFiles("./**/*.sln");

// anti-convention: Project files can be anywhere.
var projects = GetFiles("./**/*.*proj");

// anti-convention: NUnit project files can be anywhere.  (Top-level, next to build csproj, next to unit tests csproj?)
var nUnitProjectsPattern = "./**/*.nunit";

// conventions: NuSpec files are next to their corresponding project, named the same.  (This convention comes from NuGet).
var packableProjects = projects.Where(p => FileExists(p.ChangeExtension("nuspec")));

// convention: Artifact directory is named "artifacts".
var artifactDirectory = new DirectoryPath("./artifacts");

// convention: NuGet configuration is in root directory, named thus.
var nuGetConfig = "./NuGet.config";

// Setup and Teardown

Setup(() =>
{ // Executed BEFORE the first task.
  Information("Running tasks...");
});

Teardown(() =>
{ // Executed AFTER the last task.
  Information("Finished running tasks.");
});

// Task Definitions

Task("Clean") // Clean all build directories.
  .Does(() =>
  {
    foreach (var solutionPath in solutions.Select(s => s.GetDirectory()))
    {
      Information("Cleaning {0}", solutionPath);
      CleanDirectory(artifactDirectory);
      CleanDirectories(solutionPath + "/**/bin/" + configuration);
      CleanDirectories(solutionPath + "/**/obj/" + configuration);
    }
  });

Task("Restore") // Restore all NuGet packages.
  .Does(() =>
  {
    foreach (var solution in solutions)
    {
      Information("Restoring {0}...", solution);
      NuGetRestore(solution, new NuGetRestoreSettings
      {
        Verbosity = nuGetVerbosity
      });
    }
  });

Task("Build") // Build all solutions.
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")
  .Does(() =>
  {
    foreach (var solution in solutions)
    {
      Information("Building {0}...", solution);
      MSBuild(solution, new MSBuildSettings
      {
        Configuration = configuration,
        Verbosity = verbosity
      });
    }
  });

Task("Test") // Run all NUnit unit tests.
  .IsDependentOn("Build")
  .Does(() =>
  {
    CreateDirectory(artifactDirectory);
    NUnit3(nUnitProjectsPattern, new NUnit3Settings
    {
      Configuration = configuration,
      Results = artifactDirectory.CombineWithFilePath("TestResult.xml"), // note(cosborn) This is the default file name.
      Verbose = nUnitVerbosity
    });
  });

Task("Package") // Pack the produced assemblies into a NuGet package.
  .IsDependentOn("Test") // note(cosborn) This creates the artifacts directory.
  .Does(() =>
  { // todo(cosborn) The built-in NuGet packer doesn't respect project files.  Make this a Cake extension.
    // note(cosborn) Find project files that have a .nuspec of the same name next to them (which NuGet respects).
    foreach (var project in packableProjects)
    {
      Information("Packing project {0}...", project);
      StartProcess("nuget", string.Format("pack -IncludeReferencedProjects {0} -Prop Configuration={1} -OutputDirectory {2} -Verbosity {3}", project, configuration, artifactDirectory, nuGetVerbosity));
    }
  });

Task("Push-Stable") // Push the packed NuGet package to the stable repo.
  .IsDependentOn("Package")
  .Does(() =>
  {
    foreach (var package in GetFiles(artifactDirectory + "/*.nupkg"))
    {
      NuGetPush(package, new NuGetPushSettings
      {
        ConfigFile = nuGetConfig,
        Verbosity = nuGetVerbosity
      });
    }
  });

// Targets

Task("Default")
  .IsDependentOn("Test");

// Execution

RunTarget(target);
