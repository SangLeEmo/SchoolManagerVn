var cache_name = 'qlgvvncache';
var urlstocache = [
    './',
    './scripts/jquery-3.3.1.min.js',
    './content/site.css',
    './content/bootstrap.min.css',
    './scripts/bootstrap.min.js',
    './scripts/modernizr-2.8.3.js',
    './fonts/glyphicons-halflings-regular.woff2',
    './home/about',
    './home/contact',
    './home/allschoolyear',
    './home/dangkymonhoc',
    './home/danhsachhocphan',
    './home/marks',
    './home/phancong',
    './home/student',
    './home/teacher',
    './home/thongtincanhan',
    './home/thongtinmonhoc',
    './member/createstudent',
    './member/createteacher',
    './member/creategraduate',
];
self.addEventListener('install', function (event) {
    // perform install steps
    event.waituntil(
        caches.open(cache_name)
            .then(function (cache) {
                console.log('opened cache');
                return cache.addall(urlstocache);
            })
    );
});
self.addEventListener('fetch', function (event) {
    event.respondWith(
        caches.match(event.request)
            .then(function (response) {
                // cache hit - return response
                if (response) {
                    return response;
                }

                return fetch(event.request).then(
                    function (response) {
                        // check if we received a valid response
                        if (!response || response.status !== 200 || response.type !== 'basic') {
                            return response;
                        }

                        // important: clone the response. a response is a stream
                        // and because we want the browser to consume the response
                        // as well as the cache consuming the response, we need
                        // to clone it so we have two streams.
                        var responsetocache = response.clone();

                        caches.open(cache_name)
                            .then(function (cache) {
                                cache.put(event.request, responsetocache);
                            });

                        return response;
                    }
                );
            })
    );
});
self.addEventListener('activate', function (event) {

    var cachewhitelist = ['qlgvvncache'];

    event.waituntil(
        caches.keys().then(function (cachenames) {
            return promise.all(
                cachenames.map(function (cachename) {
                    if (cachewhitelist.indexof(cachename) === -1) {
                        return caches.delete(cachename);
                    }
                })
            );
        })
    );
});