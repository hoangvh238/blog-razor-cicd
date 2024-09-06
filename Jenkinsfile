node {
  stage('SCM') {
    checkout scm
  }
  stage('SonarQube Analysis') {
    def scannerHome = tool 'SonarScanner for .NET'
    withSonarQubeEnv() {
      sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"CampScholar-Scan\""
      sh "dotnet build"
      sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
    }
  }
}
