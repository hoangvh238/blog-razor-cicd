pipeline {
    agent any

    triggers {
        pollSCM('H/5 * * * *') 
    }

    stages {
        stage('Checkout') {
            steps {
                // Clone repository với thông tin của PR
                checkout scm
            }
        }

        stage('Build') {
            steps {
                // Thực hiện các bước build
                sh 'echo Building project...'
            }
        }

        stage('Test') {
            steps {
                // Chạy các bước test
                sh 'echo Running tests...'
            }
        }

        stage('Deploy') {
            steps {
                // Triển khai hoặc các bước tiếp theo
                sh 'echo Deploying project...'
            }
        }
    }

    post {
        success {
            // Báo cáo thành công sau khi build
            echo 'Build completed successfully.'
        }
        failure {
            // Báo cáo thất bại
            echo 'Build failed.'
        }
    }
}
