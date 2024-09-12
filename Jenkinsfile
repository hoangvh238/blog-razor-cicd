pipeline {
    agent any

    environment {
        SCANNER_HOME = tool 'SonarScan-dotnet'
        SONAR_HOST_URL = 'http://54.251.182.253:9000/'
        SONAR_PROJECT_KEY = 'CampScholar-Scan'
    }
    
    tools {
        dockerTool 'docker'
    }

    stages {
        stage('Check Branch') {
            steps {
                script {
                    def branchName = env.BRANCH_NAME ?: env.CHANGE_BRANCH
                    if (!branchName?.startsWith('feat/')) {
                        error "Branch '${branchName}' is not a feature branch. Skipping pipeline execution."
                    }
                }
            }
        }

        stage('Checkout Pull Request') {
            when {
                expression {
                    return env.CHANGE_ID != null
                }
            }
            steps {
                script {
                    checkout([$class: 'GitSCM', 
                        branches: [[name: "refs/pull/${env.CHANGE_ID}/head"]], 
                        userRemoteConfigs: [[url: 'https://github.com/hoangvh238/Blog-razor-page']]
                    ])
                }
            }
        }

        stage('SonarQube Analysis Begin') {
            when {
                expression {
                    return env.CHANGE_ID != null 
                }
            }
            steps {
                withSonarQubeEnv('Sonar-Server') {
                    withCredentials([string(credentialsId: 'Sonar-Token', variable: 'SONAR_TOKEN')]) {
                        sh "${SCANNER_HOME}/SonarScanner.MSBuild.dll begin /k:\"${SONAR_PROJECT_KEY}\" /d:sonar.login=${SONAR_TOKEN} /d:sonar.host.url=\"${SONAR_HOST_URL}\""
                    }
                }
            }
        }

        stage('Build') {
            when {
                expression {
                    return env.CHANGE_ID != null 
                }
            }
            steps {
                sh 'dotnet build'
            }
        }

        stage('SonarQube Analysis End') {
            when {
                expression {
                    return env.CHANGE_ID != null 
                }
            }
            steps {
                withSonarQubeEnv('Sonar-Server') {
                    withCredentials([string(credentialsId: 'Sonar-Token', variable: 'SONAR_TOKEN')]) {
                        sh "${SCANNER_HOME}/SonarScanner.MSBuild.dll end /d:sonar.login=${SONAR_TOKEN}"
                    }
                }
            }
        }

        stage('Quality Gate') {
            when {
                expression {
                    return env.CHANGE_ID != null 
                }
            }
            steps {
                script {
                    def qualityGate = waitForQualityGate()
                    if (qualityGate.status == 'OK') {
                        echo "SonarQube analysis passed, proceeding to Docker build."
                    } else {
                        error "SonarQube analysis failed: ${qualityGate.status}"
                    }
                }
            }
        }

        stage('Build and Push Docker Image') {
            when {
                expression {
                    return env.CHANGE_ID != null
                }
            }
            steps {
                script {
                    docker.withRegistry('https://index.docker.io/v1/', 'docker-cred') {
                        sh "docker build -t blog-razor:latest -f Dockerfile ."
                        
                        sh "docker tag blog-razor:latest hoangvh2388/blog-razor:latest"
                        sh "docker push hoangvh2388/blog-razor:latest"
                        
                        sh "docker system prune -f"
                    }
                }
            }
        }
    }
}
