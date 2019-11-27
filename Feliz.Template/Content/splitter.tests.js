module.exports = {
    entry: "tests/Tests.fsproj",
    outDir: "tests-js",
    babel: {
        sourceMaps: false,
        presets: [
            ["@babel/preset-react"],
            ["@babel/preset-env", { modules: "commonjs" }]
        ]
    }
}