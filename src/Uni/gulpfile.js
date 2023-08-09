/// <binding ProjectOpened='watch' />
import gulp from "gulp";

import less from "gulp-less";
import cleanCss from "gulp-clean-css";

import extReplace from "gulp-ext-replace";
import clean from "gulp-clean";

import sourcemaps from "gulp-sourcemaps";

import {exec} from "child_process";

gulp.task("less:debug",
    function () {
        return gulp.src("./wwwroot/css/**/*.less")
            .pipe(sourcemaps.init())
            .pipe(less())
            .pipe(sourcemaps.write("./", {sourceRoot: "/css"}))
            .pipe(gulp.dest("./wwwroot/css"));
    });

gulp.task("less:debug:exec",
    function () {
        return exec("gulp less:debug", (error, stdout, stderr) => {
            if (error) {
                console.error(`${error.message}`);
                return;
            }

            if (stderr) {
                console.error(`${stderr}`);
                return;
            }

            console.log(`${stdout}`);
        });
    });

gulp.task("less:minified",
    function () {
        return gulp.src("./wwwroot/css/**/*.less")
            .pipe(less())
            .pipe(cleanCss())
            .pipe(gulp.dest("./wwwroot/css"));
    });

gulp.task("less:clean",
    function () {
        return gulp.src("./wwwroot/css/**/*.less")
            .pipe(extReplace(".css"))
            .pipe(clean())
            .pipe(extReplace(".css.map"))
            .pipe(clean());
    });

gulp.task("less:release",
    gulp.series("less:clean", "less:minified"));

gulp.task("rollup:debug",
    function () {
        return exec("npx rollup -c --environment BUNDLE_TYPE:debug", (error, stdout, stderr) => {
            if (error) {
                console.error(`${error.message}`);
                return;
            }

            if (stderr) {
                console.error(`${stderr}`);
                return;
            }

            console.log(`${stdout}`);
        });
    });

gulp.task("rollup:release",
    function () {
        return exec("npx rollup -c", (error, stdout, stderr) => {
            if (error) {
                console.error(`${error.message}`);
                return;
            }

            if (stderr) {
                console.error(`${stderr}`);
                return;
            }

            console.log(`${stdout}`);
        });
    });

gulp.task("rollup:clean:js",
    function () {
        return gulp.src("./wwwroot/js/**/*.ts")
            .pipe(extReplace(".js"))
            .pipe(clean())
            .pipe(extReplace(".js.map"))
            .pipe(clean());
    });

gulp.task("rollup:clean:chunks",
    function () {
        return gulp.src("./wwwroot/js/chunks/**/*")
            .pipe(clean());
    });

gulp.task("rollup:clean",
    gulp.parallel("rollup:clean:js", "rollup:clean:chunks"));

gulp.task("clean",
    gulp.parallel("less:clean", "rollup:clean"));

gulp.task("watch",
    function () {
        gulp.watch("./wwwroot/css/**/*.less", gulp.series("less:debug:exec"));
        gulp.watch("./wwwroot/js/**/*[!.d].ts", gulp.series("rollup:debug"));
    });

export default gulp.parallel("less:release", "rollup:release");