// common utilities used by webpack
var realFs = require('fs')
var gracefulFs = require('graceful-fs')

module.exports = {
    // Patches the fs native module with the alternative graceful-fs module
    // which handles EMFILE errors internally
    patchGracefulFileSystem: () => gracefulFs.gracefulify(realFs)
}
