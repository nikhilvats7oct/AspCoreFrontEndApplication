const webpack = require('webpack');

module.exports = {
    mode: 'production',
    devtool: 'source-map',
    entry: {
        main: './app/js/main.js',
    },
    output: {
        filename: "[name].min.js",
        chunkFilename: '[name].bundle.js',
        publicPath: "/js/"
    },
    optimization: {
        minimize: false,
        splitChunks: {
            cacheGroups: {
                vendor: {
                    chunks: 'initial',
                    name: 'vendor',
                    test: 'vendor',
                    enforce: true
                }
            }
        }
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                loader: 'babel-loader',
                query: {
                    presets: ['es2015'],
                    plugins: ['babel-plugin-dynamic-import-webpack']
                }
            },
            {
                test: /\.(png|jpe?g|gif|svg)(\?.*)?$/,
                loader: 'url-loader',
                query: {
                    limit: 10000,
                },
            },
            {
                test: /\.js$/,
                exclude: /node_modules\/(?!(dom7|swiper)\/).*/,
                loader: 'babel-loader',
                query: {
                    presets: ['es2015'],
                    plugins: ['babel-plugin-dynamic-import-webpack']
                }
            }
        ]
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery"
        })
    ]
};