import rollupTs from "@rollup/plugin-typescript";
import rollupNodeResolve from "@rollup/plugin-node-resolve";
import rollupCommonJs from "@rollup/plugin-commonjs";
import rollupTerser from "@rollup/plugin-terser";
import {globSync} from "glob";
import path from "path";

function getFiles() {
    const files = globSync("./wwwroot/js/**/*[!.d].ts");
    const filesObject = {};
    for (let i = 0; i < files.length; i++) {
        filesObject[path.relative("./wwwroot/js", files[i]).replace("\\", "/").replace(/\.[^\/.]+$/, "")] = files[i];
    }
    return filesObject;
}

export const debug = {
    input: getFiles(),
    output: {
        chunkFileNames: "chunks/[name]-[hash].js",
        entryFileNames: "[name].js",
        dir: "./wwwroot/js",
        sourcemap: true
    },
    plugins: [rollupNodeResolve(), rollupCommonJs(), rollupTs()],
    watch: {
        include: "./wwwroot/js/**/*[!.d].ts"
    }
};

export const release = {
    input: getFiles(),
    output: {
        chunkFileNames: "chunks/[name]-[hash].js",
        entryFileNames: "[name].js",
        dir: "./wwwroot/js",
        sourcemap: false
    },
    plugins: [rollupNodeResolve(), rollupCommonJs(), rollupTs(), rollupTerser()],
    watch: {
        include: "./wwwroot/js/**/*[!.d].ts"
    }
};

const config = process.env.BUNDLE_TYPE === "debug" ? debug : release;

export default config;