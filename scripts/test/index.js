const $ = require('shelljs');
const path = require('path');
require('../logging')

$.config.fatal = true


try {
    require('./ops.integration.test')
} catch (e) {
    console.error(`❎ AN UNEXPECTED ERROR OCURRED`)
    console.error(e)
    process.exit(1)
}
