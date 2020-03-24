# Caps Lock #

* See the [demo](https://rawgit.com/aaditmshah/capsLock/master/demo.html "Caps Lock Demo").

* Include it on your web pages:

    ```html
    <script src="https://rawgit.com/aaditmshah/capsLock/master/capsLock.js"></script>
    ```

* Install it using [npm](https://www.npmjs.org/ "npm"):

    ```bash
    npm install capslock
    ```

* Build it using [browserify](http://browserify.org/ "Browserify").

* Import it as a node module:

    ```javascript
    var capsLock = require("capslock");
    ```

* Use it as follows:

    ```javascript
    alert(capsLock.status);

    capsLock.observe(function (status) {
        alert(status);
    });
    ```

## License ##

    The MIT License (MIT)

    Copyright (c) 2014 Aadit M Shah

    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.