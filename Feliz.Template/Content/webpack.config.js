// Template for webpack.config.js in Fable projects
// In most cases, you'll only need to edit the CONFIG object (after dependencies)
// See below if you need better fine-tuning of Webpack options

// Dependencies. Also required: core-js, @babel/core,
// @babel/preset-env, babel-loader, sass, sass-loader, css-loader, style-loader, file-loader
var path = require("path");
var webpack = require("webpack");
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');
var MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ReactRefreshWebpackPlugin = require('@pmmmwh/react-refresh-webpack-plugin');
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
        indexHtmlTemplate: "./src/index.html",
        fsharpEntry: "./src/Main.fs.js",
        outputDir: "./dist",
        assetsDir: "./public",
        devServerPort: 8080,
        // When using webpack-dev-server, you may need to redirect some calls
        // to a external API server. See https://webpack.js.org/configuration/dev-server/#devserver-proxy
        devServerProxy: {
            '/api': {
                // assuming the backend is running on port 5000
                // so that every request starting with /api will be proxied here
                target: "http://localhost:5000",
                changeOrigin: true
            }
        },
        // Use babel-preset-env to generate JS compatible with most-used browsers.
        // More info at https://babeljs.io/docs/en/next/babel-preset-env.html
        babel: {
            plugins: [isDevelopment && require.resolve('react-refresh/babel')].filter(Boolean),
            presets: ["@babel/preset-env", "@babel/preset-react"]
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
        // Besides the HtmlPlugin, we use the following plugins:
        // PRODUCTION
        //      - MiniCssExtractPlugin: Extracts CSS from bundle to a different file
        //          To minify CSS, see https://github.com/webpack-contrib/mini-css-extract-plugin#minimizing-for-production
        //      - CopyWebpackPlugin: Copies static assets to output directory
        // DEVELOPMENT
        //      - HotModuleReplacementPlugin: Enables hot reloading when code changes without refreshing
        plugins: isProduction ?
            commonPlugins.concat([
                new MiniCssExtractPlugin({ filename: 'style.css' }),
                new CopyWebpackPlugin({
                    patterns: [
                        { from: resolve(CONFIG.assetsDir) }
                    ]
                }),
            ])
            : commonPlugins.concat([
                new ReactRefreshWebpackPlugin()
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
                    },
                },
                {
                    test: /\.(sass|scss|css)$/,
                    use: [
                        isProduction
                            ? MiniCssExtractPlugin.loader
                            : 'style-loader',
                        {
                            loader: 'css-loader',
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
