const $ = require('shelljs')
const path = require('path')
require('../logging')

$.config.fatal = true
const root = path.resolve(`${__dirname}/../..`)

function deploy_to_docker(source, target) {
    const settings_script = require('./../deploy/deploy_settings')
    const settings = settings_script.get_deployment_settings()

    let docker_options;
    for (const prop in settings) {
        for (const deployment of settings[prop]) {
            if (deployment.type === 'docker') {
                docker_options = deployment.options;
            }
        }
    }

    const docker_deploy_script = require('./../deploy/deploy_docker_registry')
    docker_deploy_script.deploy(source, target, docker_options.user, docker_options.pass)
}

try {
    console.info(`building Docker images`)

    $.cd(root)

    console.debug('building the solution with "Release" configuration and "vivid:solution" tag')
    $.exec(
        `docker build --tag botops-vivid:solution --no-cache --target solution-build --build-arg configuration=Release .`
    )

    console.debug('building the final web app with "botops-vivid:latest" tag')
    $.exec(`docker build --tag botops-vivid --target final .`)

    console.debug('pushing images to the Docker hub')
    deploy_to_docker('botops-vivid:solution', 'zevere/botops-vivid:unstable-solution')
    deploy_to_docker('botops-vivid:latest', 'zevere/botops-vivid:unstable')
} catch (e) {
    console.error(e)
    process.exit(1)
}

console.info(`✅ Build succeeded`)
