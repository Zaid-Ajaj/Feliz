var path = require("path");

module.exports = function (evn, argv) {
    var mode = argv.mode || "development";
    var isProduction = mode === "production";
    console.log("Webpack mode: " + mode);

    return {
        mode: mode,
        devtool: isProduction ? false : "eval-source-map",
        entry: './tests/Tests.fsproj',
        output: {
            filename: 'tests.js',
            path: path.join(__dirname, './public-tests'),
        },
        devServer: {
            contentBase: './public-tests',
            port: 8080
        },
        module: {
            rules: [
                {
                    test: /\.fs(x|proj)?$/,
                    use: "fable-loader"
                }
            ]
        }
    };
}