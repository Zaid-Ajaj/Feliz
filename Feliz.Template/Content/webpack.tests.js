// Template for webpack.config.js in Fable projects
// In most cases, you'll only need to edit the CONFIG object (after dependencies)
// See below if you need better fine-tuning of Webpack options

// Dependencies. Also required: core-js, @babel/core,
// @babel/preset-env, babel-loader
var path = require("path");
var webpack = require("webpack");
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');
var MiniCssExtractPlugin = require("mini-css-extract-plugin");
const Dotenv = require('dotenv-webpack');
const { patchGracefulFileSystem } = require("./webpack.common.js");
patchGracefulFileSystem();

module.exports = (env, argv) => {
    const isProduction = argv.mode === 'production'
    const isDevelopment = argv.mode === 'development'
    console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

    var CONFIG = {
        // The tags to include the generated JS and CSS will be automatically injected in the HTML template
        // See https://github.com/jantimon/html-webpack-plugin
        indexHtmlTemplate: "./tests/index.html",
        fsharpEntry: "./tests/Tests.fs.js",
        outputDir: "./dist",
        assetsDir: "./public",
        devServerPort: 8085,
        // When using webpack-dev-server, you may need to redirect some calls
        // to a external API server. See https://webpack.js.org/configuration/dev-server/#devserver-proxy
        devServerProxy: undefined,
        // Use babel-preset-env to generate JS compatible with most-used browsers.
        // More info at https://babeljs.io/docs/en/next/babel-preset-env.html
        babel: {
            presets: [
                // In case interop is used with React/Jsx components, this React preset would be required
                ["@babel/preset-react"],
                ["@babel/preset-env", {
                    "targets": "> 0.25%, not dead",
                    "modules": false,
                    // This adds polyfills when needed. Requires core-js dependency.
                    // See https://babeljs.io/docs/en/babel-preset-env#usebuiltins
                    "useBuiltIns": "usage",
                    "corejs": 3
                }]
            ],
        }
    }

    // The HtmlWebpackPlugin allows us to use a template for the index.html page
    // and automatically injects <script> or <link> tags for generated bundles.
    var commonPlugins = [
        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: resolve(CONFIG.indexHtmlTemplate)
        }),

        new Dotenv({
            path: "./.env",
            silent: false,
            systemvars: true
        })
    ];

    return {
        // In development, bundle styles together with the code so they can also
        // trigger hot reloads. In production, put them in a separate CSS file.
        entry: {
            app: [resolve(CONFIG.fsharpEntry)]
        },
        // Add a hash to the output file name in production
        // to prevent browser caching if code changes
        output: {
            path: resolve(CONFIG.outputDir),
            filename: isProduction ? '[name].[contenthash].js' : '[name].js'
        },
        devtool: isProduction ? "source-map" : "eval-source-map",
        optimization: {
            // Split the code coming from npm packages into a different file.
            // 3rd party dependencies change less often, let the browser cache them.
            splitChunks: {
                cacheGroups: {
                    commons: {
                        test: /node_modules/,
                        name: "vendors",
                        chunks: "all"
                    }
                }
            },
        },
        plugins: commonPlugins.concat([
            new CopyWebpackPlugin({
                patterns: [
                    { from: resolve(CONFIG.assetsDir) }
                ]
            }),
        ]),

        resolve: {
            // See https://github.com/fable-compiler/Fable/issues/1490
            symlinks: false,
            modules: [resolve("./node_modules")],
            alias: {
                // Some old libraries still use an old specific version of core-js
                // Redirect the imports of these libraries to the newer core-js
                'core-js/es6': 'core-js/es'
            }
        },
        // Configuration for webpack-dev-server
        devServer: {
            publicPath: "/",
            contentBase: resolve(CONFIG.assetsDir),
            port: CONFIG.devServerPort,
            proxy: CONFIG.devServerProxy,
            hot: true,
            inline: true
        },
        // - fable-loader: transforms F# into JS
        // - babel-loader: transforms JS to old syntax (compatible with old browsers)
        // - sass-loaders: transforms SASS/SCSS into JS
        // - file-loader: Moves files referenced in the code (fonts, images) into output folder
        module: {
            rules: [
                {
                    test: /\.(js|jsx)$/,
                    exclude: /node_modules/,
                    use: {
                        loader: 'babel-loader',
                        options: CONFIG.babel
                    }
                },
                {
                    test: /\.(sass|scss|css)$/,
                    use: [
                        isProduction
                            ? MiniCssExtractPlugin.loader
                            : 'style-loader',
                        {
                            loader: 'css-loader'
                        },
                        {
                            loader: 'sass-loader',
                            options: { implementation: require("sass") }
                        }
                    ],
                },
                {
                    test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|ttf|eot)(\?.*)?$/,
                    use: ["file-loader"]
                }
            ]
        }
    }
};

function resolve(filePath) {
    return path.isAbsolute(filePath) ? filePath : path.join(__dirname, filePath);
}
