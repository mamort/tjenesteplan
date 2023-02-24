const webpack = require('webpack');
const merge = require('webpack-merge');
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');
const common = require('./webpack.common.js');

module.exports = merge(common, {
    devtool: 'source-map',
    plugins: [
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': JSON.stringify('production'),
            'process.env.REACT_APP_API_URL': JSON.stringify(process.env.REACT_APP_API_URL)
        }),
        new UglifyJSPlugin({
            sourceMap: true
        })
    ]
});