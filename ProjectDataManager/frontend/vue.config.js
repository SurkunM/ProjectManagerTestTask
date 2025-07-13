const { defineConfig } = require("@vue/cli-service");
const path = require("path");

module.exports = defineConfig({
    outputDir: path.resolve(__dirname, "..", "wwwroot"),

    devServer: {
        proxy: {
            "^/api": {
                target: "https://localhost:44340/"
            }
        }
    },
    transpileDependencies: true
})
