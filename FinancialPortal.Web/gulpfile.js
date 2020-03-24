var gulp = require('gulp'),
    sourceMaps = require('gulp-sourcemaps'),
    del = require('del'),
    browserSync = require('browser-sync').create(),
    reload = browserSync.reload,
    webpack = require('webpack'),
    webpackStream = require('webpack-stream'),
    concat = require("gulp-concat"),
    minify = require("gulp-minify"),
    webpackConfig = require('./webpack.config');

var basePaths = {
    src: 'app/',
    build: 'wwwroot/'
};

// Cleaners
gulp.task('clean-imgs', function (cb) {
    //del.sync([
    //    basePaths.build + 'images/**/*'
    //], {
    //        force: true
    //    });
    cb();
});
gulp.task('clean-videos', function (cb) {
    //del.sync([
    //    basePaths.build + 'video/**/*'
    //], {
    //        force: true
    //    });
    cb();
});
gulp.task('clean-fonts', function (cb) {
    //del.sync([
    //    basePaths.build + 'fonts/**/*'
    //], {
    //        force: true
    //    });
    cb();
});
gulp.task('clean-scripts', function (cb) {
    //del.sync([
    //    basePaths.build + 'js/lib/**/*'
    //], {
    //        force: true
    //    });
    cb();
});

// Compile task
gulp.task('compile-sass', function () {
    var sass = require('gulp-sass'),
        autoprefixer = require('gulp-autoprefixer'),
        cssimport = require('gulp-cssimport');

    return gulp.src([basePaths.src + 'scss/main.scss', basePaths.src + 'scss/admin-styles.scss'])
        .pipe(sourceMaps.init())
        .pipe(sass({
            includePaths: [
                // NOTE: paths have to be defined twice, one set for use here, another for when the framework is
                //        required from another project.
                './node_modules/',
                './../',
            ],
            sourceMap: true,
            sourceMapContents: true,
            sourceMapEmbed: true,
            outputStyle: 'compressed'
        }).on('error', sass.logError))
        .pipe(cssimport({
            matchPattern: '*.css',
            includePaths: [
                './node_modules/',
                './../'
            ]
        }))
        .pipe(autoprefixer(['last 2 versions', 'iOS 7', 'ie 10', 'safari 9']))
        .pipe(sourceMaps.write())
        .pipe(gulp.dest(basePaths.build + 'css/'))
        .pipe(reload({ stream: true }));
});

gulp.task('bundle-js', function () {
    return gulp.src(basePaths.src + 'js/main.js')
        .pipe(webpackStream(webpackConfig, webpack)).on('error', function (err) {
            console.log(err.toString());
            this.emit('end');
        })
        .pipe(gulp.dest(basePaths.build + 'js/'))
        .pipe(reload({ stream: true }));
});

gulp.task('minify-imgs', ['copy-svg', 'clean-imgs'], function () {
    var imagemin = require('gulp-imagemin'),
        plumber = require('gulp-plumber'),
        gutil = require('gulp-util');

    return gulp.src(basePaths.src + 'images/**/*.{png,gif,jpg}')
        .pipe(plumber())
        .pipe(imagemin({
            progressive: true
        }).on('error', gutil.log))
        .pipe(gulp.dest(basePaths.build + 'images/'));
});

gulp.task('copy-svg', function () {
    return gulp.src(basePaths.src + 'images/**/*.svg')
        .pipe(gulp.dest(basePaths.build + 'images/'));
});

gulp.task('copy-videos', ['clean-videos'], function () {
    return gulp.src(basePaths.src + 'video/**/*')
        .pipe(gulp.dest(basePaths.build + 'video/'));
});

gulp.task('copy-fonts', ['clean-fonts'], function () {
    return gulp.src(basePaths.src + 'fonts/**/*')
        .pipe(gulp.dest(basePaths.build + 'fonts/'));
});

gulp.task('copy-scripts', ['clean-scripts'], function () {
    return gulp.src(basePaths.src + 'js/lib/**')
        .pipe(gulp.dest(basePaths.build + 'js/lib'));
});

gulp.task('copy-vendor-scripts', ['copy-jquery-validation', 'copy-jquery-validation-unobtrusive']);

gulp.task('copy-jquery-validation', function () {
    return gulp.src('node_modules/jquery-validation/dist/jquery.validate.min.js')
        .pipe(gulp.dest(basePaths.build + 'js/lib'));
});

gulp.task('copy-jquery-validation-unobtrusive', function () {
    return gulp.src('node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js')
        .pipe(gulp.dest(basePaths.build + 'js/lib'));
});

gulp.task('additional-script-bundle', function () {
    gulp.src(['node_modules/jquery-validation/dist/jquery.validate.min.js', 'node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js', basePaths.src + "js/additional-methods.js", basePaths.src + 'js/validators.js'])
        .pipe(concat('additionalscript.js'))
        .pipe(minify())
        .pipe(gulp.dest(basePaths.build + 'js/'));
});

gulp.task('copy-validators-js', function () {
    return gulp.src(basePaths.src + "js/validators.js")
        .pipe(gulp.dest(basePaths.build + 'js/'));
});


gulp.task('watch', ['build'], function () {
    var watch = require('gulp-watch');

    watch(basePaths.src + 'js/**.js', function () {
        gulp.start('bundle-js');
        //gulp.start('copy-lib-js');
    });
    watch(basePaths.src + 'scss/**/*.scss', function () {
        gulp.start('compile-sass');
    });
    watch(basePaths.src + 'images/**/*.{png,gif,jpg,svg}', function () {
        gulp.start('minify-imgs');
    });
});

gulp.task('build', ['copy-fonts', 'copy-scripts', 'bundle-js', 'compile-sass', 'minify-imgs', 'copy-videos', 'copy-vendor-scripts', 'additional-script-bundle']);

// Watch task
gulp.task('default', ['watch']);
