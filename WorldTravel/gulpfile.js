var gulp = require("gulp");

var uglify = require("gulp-minify");


var ngAnnotate = require("gulp-ng-annotate");



gulp.task('minify', function () {

    return gulp.src("wwwroot/js/*.js")
        .pipe(ngAnnotate())
        .pipe(uglify({
        ext:{
            src:'-debug.js',
            min:'.js'
        },
        exclude: ['tasks'],
        ignoreFiles: ['.combo.js', '-min.js']
    }))
     .pipe(gulp.dest('wwwroot/lib/_app')) ; 
       


});