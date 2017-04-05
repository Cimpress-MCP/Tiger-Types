pipeline {
  agent {
    docker {
      image 'tiger/dotnet:exp'
    }
  }
  
  options {
    timestamps()
  }
  
  stages {
    stage('Restore') {
      steps {
        sh 'dotnet restore'
      }
    }
    stage('Build') {
      steps {
        sh 'dotnet build -c Release'
      }
    }
    stage('Test') {
      steps {
        sh 'dotnet test -c Release ./unit/Test.csproj'
      }
    }
    stage('Pack') {
      steps {
        sh 'dotnet pack -c Release -o "$(pwd)/artifacts"'
      }
    }
    stage('Deploy') {
      when  { branch 'master' }
      environment {
        NUGET_API_KEY = credentials('NuGet')
      }
      steps {
        sh 'dotnet nuget push artifacts/*.nupkg -k "${NUGET_API_KEY}"'
      }
    }
  }

  post {
    always {
      deleteDir()
    }
  }
}
