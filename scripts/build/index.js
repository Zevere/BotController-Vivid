const $ = require('shelljs')
const path = require('path')
require('../logging')

$.config.fatal = true
const root = path.resolve(`${__dirname}/../..`)


try {
    console.info(`building Docker images`)

    $.cd(root)

    console.debug('building the solution with "Debug" configuration and "vivid:debug" tag')
    $.exec(`docker build --tag vivid:debug --no-cache --target solution-build  .`)

    console.debug('building the final web app with "botops-vivid:latest" tag')
    $.exec(`docker build --tag botops-vivid --target final .`)
} catch (e) {
    console.error(e)
    process.exit(1)
}

console.info(`✅ Build succeeded`)
