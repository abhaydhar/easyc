/*
CryptoJS v3.1.2
code.google.com/p/crypto-js
(c) 2009-2013 by Jeff Mott. All rights reserved.
code.google.com/p/crypto-js/wiki/License
*/
var CryptoJS = CryptoJS || function (h, r) {
    var k = {}, l = k.lib = {}, n = function () { }, f = l.Base = { extend: function (a) { n.prototype = this; var b = new n; a && b.mixIn(a); b.hasOwnProperty("init") || (b.init = function () { b.$super.init.apply(this, arguments) }); b.init.prototype = b; b.$super = this; return b }, create: function () { var a = this.extend(); a.init.apply(a, arguments); return a }, init: function () { }, mixIn: function (a) { for (var b in a) a.hasOwnProperty(b) && (this[b] = a[b]); a.hasOwnProperty("toString") && (this.toString = a.toString) }, clone: function () { return this.init.prototype.extend(this) } },
    j = l.WordArray = f.extend({
        init: function (a, b) { a = this.words = a || []; this.sigBytes = b != r ? b : 4 * a.length }, toString: function (a) { return (a || s).stringify(this) }, concat: function (a) { var b = this.words, d = a.words, c = this.sigBytes; a = a.sigBytes; this.clamp(); if (c % 4) for (var e = 0; e < a; e++) b[c + e >>> 2] |= (d[e >>> 2] >>> 24 - 8 * (e % 4) & 255) << 24 - 8 * ((c + e) % 4); else if (65535 < d.length) for (e = 0; e < a; e += 4) b[c + e >>> 2] = d[e >>> 2]; else b.push.apply(b, d); this.sigBytes += a; return this }, clamp: function () {
            var a = this.words, b = this.sigBytes; a[b >>> 2] &= 4294967295 <<
            32 - 8 * (b % 4); a.length = h.ceil(b / 4)
        }, clone: function () { var a = f.clone.call(this); a.words = this.words.slice(0); return a }, random: function (a) { for (var b = [], d = 0; d < a; d += 4) b.push(4294967296 * h.random() | 0); return new j.init(b, a) }
    }), m = k.enc = {}, s = m.Hex = {
        stringify: function (a) { var b = a.words; a = a.sigBytes; for (var d = [], c = 0; c < a; c++) { var e = b[c >>> 2] >>> 24 - 8 * (c % 4) & 255; d.push((e >>> 4).toString(16)); d.push((e & 15).toString(16)) } return d.join("") }, parse: function (a) {
            for (var b = a.length, d = [], c = 0; c < b; c += 2) d[c >>> 3] |= parseInt(a.substr(c,
            2), 16) << 24 - 4 * (c % 8); return new j.init(d, b / 2)
        }
    }, p = m.Latin1 = { stringify: function (a) { var b = a.words; a = a.sigBytes; for (var d = [], c = 0; c < a; c++) d.push(String.fromCharCode(b[c >>> 2] >>> 24 - 8 * (c % 4) & 255)); return d.join("") }, parse: function (a) { for (var b = a.length, d = [], c = 0; c < b; c++) d[c >>> 2] |= (a.charCodeAt(c) & 255) << 24 - 8 * (c % 4); return new j.init(d, b) } }, t = m.Utf8 = { stringify: function (a) { try { return decodeURIComponent(escape(p.stringify(a))) } catch (b) { throw Error("Malformed UTF-8 data"); } }, parse: function (a) { return p.parse(unescape(encodeURIComponent(a))) } },
    q = l.BufferedBlockAlgorithm = f.extend({
        reset: function () { this._data = new j.init; this._nDataBytes = 0 }, _append: function (a) { "string" == typeof a && (a = t.parse(a)); this._data.concat(a); this._nDataBytes += a.sigBytes }, _process: function (a) { var b = this._data, d = b.words, c = b.sigBytes, e = this.blockSize, f = c / (4 * e), f = a ? h.ceil(f) : h.max((f | 0) - this._minBufferSize, 0); a = f * e; c = h.min(4 * a, c); if (a) { for (var g = 0; g < a; g += e) this._doProcessBlock(d, g); g = d.splice(0, a); b.sigBytes -= c } return new j.init(g, c) }, clone: function () {
            var a = f.clone.call(this);
            a._data = this._data.clone(); return a
        }, _minBufferSize: 0
    }); l.Hasher = q.extend({
        cfg: f.extend(), init: function (a) { this.cfg = this.cfg.extend(a); this.reset() }, reset: function () { q.reset.call(this); this._doReset() }, update: function (a) { this._append(a); this._process(); return this }, finalize: function (a) { a && this._append(a); return this._doFinalize() }, blockSize: 16, _createHelper: function (a) { return function (b, d) { return (new a.init(d)).finalize(b) } }, _createHmacHelper: function (a) {
            return function (b, d) {
                return (new u.HMAC.init(a,
                d)).finalize(b)
            }
        }
    }); var u = k.algo = {}; return k
}(Math);

/*
CryptoJS v3.1.2
code.google.com/p/crypto-js
(c) 2009-2013 by Jeff Mott. All rights reserved.
code.google.com/p/crypto-js/wiki/License
*/
(function () {
    var h = CryptoJS, j = h.lib.WordArray; h.enc.Base64 = {
        stringify: function (b) { var e = b.words, f = b.sigBytes, c = this._map; b.clamp(); b = []; for (var a = 0; a < f; a += 3) for (var d = (e[a >>> 2] >>> 24 - 8 * (a % 4) & 255) << 16 | (e[a + 1 >>> 2] >>> 24 - 8 * ((a + 1) % 4) & 255) << 8 | e[a + 2 >>> 2] >>> 24 - 8 * ((a + 2) % 4) & 255, g = 0; 4 > g && a + 0.75 * g < f; g++) b.push(c.charAt(d >>> 6 * (3 - g) & 63)); if (e = c.charAt(64)) for (; b.length % 4;) b.push(e); return b.join("") }, parse: function (b) {
            var e = b.length, f = this._map, c = f.charAt(64); c && (c = b.indexOf(c), -1 != c && (e = c)); for (var c = [], a = 0, d = 0; d <
            e; d++) if (d % 4) { var g = f.indexOf(b.charAt(d - 1)) << 2 * (d % 4), h = f.indexOf(b.charAt(d)) >>> 6 - 2 * (d % 4); c[a >>> 2] |= (g | h) << 24 - 8 * (a % 4); a++ } return j.create(c, a)
        }, _map: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="
    }
})();

/*
CryptoJS v3.1.2
code.google.com/p/crypto-js
(c) 2009-2013 by Jeff Mott. All rights reserved.
code.google.com/p/crypto-js/wiki/License
*/
var CryptoJS = CryptoJS || function (h, s) {
    var f = {}, g = f.lib = {}, q = function () { }, m = g.Base = { extend: function (a) { q.prototype = this; var c = new q; a && c.mixIn(a); c.hasOwnProperty("init") || (c.init = function () { c.$super.init.apply(this, arguments) }); c.init.prototype = c; c.$super = this; return c }, create: function () { var a = this.extend(); a.init.apply(a, arguments); return a }, init: function () { }, mixIn: function (a) { for (var c in a) a.hasOwnProperty(c) && (this[c] = a[c]); a.hasOwnProperty("toString") && (this.toString = a.toString) }, clone: function () { return this.init.prototype.extend(this) } },
    r = g.WordArray = m.extend({
        init: function (a, c) { a = this.words = a || []; this.sigBytes = c != s ? c : 4 * a.length }, toString: function (a) { return (a || k).stringify(this) }, concat: function (a) { var c = this.words, d = a.words, b = this.sigBytes; a = a.sigBytes; this.clamp(); if (b % 4) for (var e = 0; e < a; e++) c[b + e >>> 2] |= (d[e >>> 2] >>> 24 - 8 * (e % 4) & 255) << 24 - 8 * ((b + e) % 4); else if (65535 < d.length) for (e = 0; e < a; e += 4) c[b + e >>> 2] = d[e >>> 2]; else c.push.apply(c, d); this.sigBytes += a; return this }, clamp: function () {
            var a = this.words, c = this.sigBytes; a[c >>> 2] &= 4294967295 <<
            32 - 8 * (c % 4); a.length = h.ceil(c / 4)
        }, clone: function () { var a = m.clone.call(this); a.words = this.words.slice(0); return a }, random: function (a) { for (var c = [], d = 0; d < a; d += 4) c.push(4294967296 * h.random() | 0); return new r.init(c, a) }
    }), l = f.enc = {}, k = l.Hex = {
        stringify: function (a) { var c = a.words; a = a.sigBytes; for (var d = [], b = 0; b < a; b++) { var e = c[b >>> 2] >>> 24 - 8 * (b % 4) & 255; d.push((e >>> 4).toString(16)); d.push((e & 15).toString(16)) } return d.join("") }, parse: function (a) {
            for (var c = a.length, d = [], b = 0; b < c; b += 2) d[b >>> 3] |= parseInt(a.substr(b,
            2), 16) << 24 - 4 * (b % 8); return new r.init(d, c / 2)
        }
    }, n = l.Latin1 = { stringify: function (a) { var c = a.words; a = a.sigBytes; for (var d = [], b = 0; b < a; b++) d.push(String.fromCharCode(c[b >>> 2] >>> 24 - 8 * (b % 4) & 255)); return d.join("") }, parse: function (a) { for (var c = a.length, d = [], b = 0; b < c; b++) d[b >>> 2] |= (a.charCodeAt(b) & 255) << 24 - 8 * (b % 4); return new r.init(d, c) } }, j = l.Utf8 = { stringify: function (a) { try { return decodeURIComponent(escape(n.stringify(a))) } catch (c) { throw Error("Malformed UTF-8 data"); } }, parse: function (a) { return n.parse(unescape(encodeURIComponent(a))) } },
    u = g.BufferedBlockAlgorithm = m.extend({
        reset: function () { this._data = new r.init; this._nDataBytes = 0 }, _append: function (a) { "string" == typeof a && (a = j.parse(a)); this._data.concat(a); this._nDataBytes += a.sigBytes }, _process: function (a) { var c = this._data, d = c.words, b = c.sigBytes, e = this.blockSize, f = b / (4 * e), f = a ? h.ceil(f) : h.max((f | 0) - this._minBufferSize, 0); a = f * e; b = h.min(4 * a, b); if (a) { for (var g = 0; g < a; g += e) this._doProcessBlock(d, g); g = d.splice(0, a); c.sigBytes -= b } return new r.init(g, b) }, clone: function () {
            var a = m.clone.call(this);
            a._data = this._data.clone(); return a
        }, _minBufferSize: 0
    }); g.Hasher = u.extend({
        cfg: m.extend(), init: function (a) { this.cfg = this.cfg.extend(a); this.reset() }, reset: function () { u.reset.call(this); this._doReset() }, update: function (a) { this._append(a); this._process(); return this }, finalize: function (a) { a && this._append(a); return this._doFinalize() }, blockSize: 16, _createHelper: function (a) { return function (c, d) { return (new a.init(d)).finalize(c) } }, _createHmacHelper: function (a) {
            return function (c, d) {
                return (new t.HMAC.init(a,
                d)).finalize(c)
            }
        }
    }); var t = f.algo = {}; return f
}(Math);
(function (h) {
    for (var s = CryptoJS, f = s.lib, g = f.WordArray, q = f.Hasher, f = s.algo, m = [], r = [], l = function (a) { return 4294967296 * (a - (a | 0)) | 0 }, k = 2, n = 0; 64 > n;) { var j; a: { j = k; for (var u = h.sqrt(j), t = 2; t <= u; t++) if (!(j % t)) { j = !1; break a } j = !0 } j && (8 > n && (m[n] = l(h.pow(k, 0.5))), r[n] = l(h.pow(k, 1 / 3)), n++); k++ } var a = [], f = f.SHA256 = q.extend({
        _doReset: function () { this._hash = new g.init(m.slice(0)) }, _doProcessBlock: function (c, d) {
            for (var b = this._hash.words, e = b[0], f = b[1], g = b[2], j = b[3], h = b[4], m = b[5], n = b[6], q = b[7], p = 0; 64 > p; p++) {
                if (16 > p) a[p] =
                c[d + p] | 0; else { var k = a[p - 15], l = a[p - 2]; a[p] = ((k << 25 | k >>> 7) ^ (k << 14 | k >>> 18) ^ k >>> 3) + a[p - 7] + ((l << 15 | l >>> 17) ^ (l << 13 | l >>> 19) ^ l >>> 10) + a[p - 16] } k = q + ((h << 26 | h >>> 6) ^ (h << 21 | h >>> 11) ^ (h << 7 | h >>> 25)) + (h & m ^ ~h & n) + r[p] + a[p]; l = ((e << 30 | e >>> 2) ^ (e << 19 | e >>> 13) ^ (e << 10 | e >>> 22)) + (e & f ^ e & g ^ f & g); q = n; n = m; m = h; h = j + k | 0; j = g; g = f; f = e; e = k + l | 0
            } b[0] = b[0] + e | 0; b[1] = b[1] + f | 0; b[2] = b[2] + g | 0; b[3] = b[3] + j | 0; b[4] = b[4] + h | 0; b[5] = b[5] + m | 0; b[6] = b[6] + n | 0; b[7] = b[7] + q | 0
        }, _doFinalize: function () {
            var a = this._data, d = a.words, b = 8 * this._nDataBytes, e = 8 * a.sigBytes;
            d[e >>> 5] |= 128 << 24 - e % 32; d[(e + 64 >>> 9 << 4) + 14] = h.floor(b / 4294967296); d[(e + 64 >>> 9 << 4) + 15] = b; a.sigBytes = 4 * d.length; this._process(); return this._hash
        }, clone: function () { var a = q.clone.call(this); a._hash = this._hash.clone(); return a }
    }); s.SHA256 = q._createHelper(f); s.HmacSHA256 = q._createHmacHelper(f)
})(Math);
(function () {
    var h = CryptoJS, s = h.enc.Utf8; h.algo.HMAC = h.lib.Base.extend({
        init: function (f, g) { f = this._hasher = new f.init; "string" == typeof g && (g = s.parse(g)); var h = f.blockSize, m = 4 * h; g.sigBytes > m && (g = f.finalize(g)); g.clamp(); for (var r = this._oKey = g.clone(), l = this._iKey = g.clone(), k = r.words, n = l.words, j = 0; j < h; j++) k[j] ^= 1549556828, n[j] ^= 909522486; r.sigBytes = l.sigBytes = m; this.reset() }, reset: function () { var f = this._hasher; f.reset(); f.update(this._iKey) }, update: function (f) { this._hasher.update(f); return this }, finalize: function (f) {
            var g =
            this._hasher; f = g.finalize(f); g.reset(); return g.finalize(this._oKey.clone().concat(f))
        }
    })
})();


//! moment-timezone.js
//! version : 0.5.4
//! author : Tim Wood
//! license : MIT
//! github.com/moment/moment-timezone
!function (a, b) { "use strict"; "function" == typeof define && define.amd ? define(["moment"], b) : "object" == typeof module && module.exports ? module.exports = b(require("moment")) : b(a.moment) }(this, function (a) { "use strict"; function b(a) { return a > 96 ? a - 87 : a > 64 ? a - 29 : a - 48 } function c(a) { var c, d = 0, e = a.split("."), f = e[0], g = e[1] || "", h = 1, i = 0, j = 1; for (45 === a.charCodeAt(0) && (d = 1, j = -1), d; d < f.length; d++) c = b(f.charCodeAt(d)), i = 60 * i + c; for (d = 0; d < g.length; d++) h /= 60, c = b(g.charCodeAt(d)), i += c * h; return i * j } function d(a) { for (var b = 0; b < a.length; b++) a[b] = c(a[b]) } function e(a, b) { for (var c = 0; b > c; c++) a[c] = Math.round((a[c - 1] || 0) + 6e4 * a[c]); a[b - 1] = 1 / 0 } function f(a, b) { var c, d = []; for (c = 0; c < b.length; c++) d[c] = a[b[c]]; return d } function g(a) { var b = a.split("|"), c = b[2].split(" "), g = b[3].split(""), h = b[4].split(" "); return d(c), d(g), d(h), e(h, g.length), { name: b[0], abbrs: f(b[1].split(" "), g), offsets: f(c, g), untils: h, population: 0 | b[5] } } function h(a) { a && this._set(g(a)) } function i(a) { var b = a.toTimeString(), c = b.match(/\([a-z ]+\)/i); c && c[0] ? (c = c[0].match(/[A-Z]/g), c = c ? c.join("") : void 0) : (c = b.match(/[A-Z]{3,5}/g), c = c ? c[0] : void 0), "GMT" === c && (c = void 0), this.at = +a, this.abbr = c, this.offset = a.getTimezoneOffset() } function j(a) { this.zone = a, this.offsetScore = 0, this.abbrScore = 0 } function k(a, b) { for (var c, d; d = 6e4 * ((b.at - a.at) / 12e4 | 0) ;) c = new i(new Date(a.at + d)), c.offset === a.offset ? a = c : b = c; return a } function l() { var a, b, c, d = (new Date).getFullYear() - 2, e = new i(new Date(d, 0, 1)), f = [e]; for (c = 1; 48 > c; c++) b = new i(new Date(d, c, 1)), b.offset !== e.offset && (a = k(e, b), f.push(a), f.push(new i(new Date(a.at + 6e4)))), e = b; for (c = 0; 4 > c; c++) f.push(new i(new Date(d + c, 0, 1))), f.push(new i(new Date(d + c, 6, 1))); return f } function m(a, b) { return a.offsetScore !== b.offsetScore ? a.offsetScore - b.offsetScore : a.abbrScore !== b.abbrScore ? a.abbrScore - b.abbrScore : b.zone.population - a.zone.population } function n(a, b) { var c, e; for (d(b), c = 0; c < b.length; c++) e = b[c], I[e] = I[e] || {}, I[e][a] = !0 } function o(a) { var b, c, d, e = a.length, f = {}, g = []; for (b = 0; e > b; b++) { d = I[a[b].offset] || {}; for (c in d) d.hasOwnProperty(c) && (f[c] = !0) } for (b in f) f.hasOwnProperty(b) && g.push(H[b]); return g } function p() { try { var a = Intl.DateTimeFormat().resolvedOptions().timeZone; if (a) { var b = H[r(a)]; if (b) return b; z("Moment Timezone found " + a + " from the Intl api, but did not have that data loaded.") } } catch (c) { } var d, e, f, g = l(), h = g.length, i = o(g), k = []; for (e = 0; e < i.length; e++) { for (d = new j(t(i[e]), h), f = 0; h > f; f++) d.scoreOffsetAt(g[f]); k.push(d) } return k.sort(m), k.length > 0 ? k[0].zone.name : void 0 } function q(a) { return D && !a || (D = p()), D } function r(a) { return (a || "").toLowerCase().replace(/\//g, "_") } function s(a) { var b, c, d, e; for ("string" == typeof a && (a = [a]), b = 0; b < a.length; b++) d = a[b].split("|"), c = d[0], e = r(c), F[e] = a[b], H[e] = c, d[5] && n(e, d[2].split(" ")) } function t(a, b) { a = r(a); var c, d = F[a]; return d instanceof h ? d : "string" == typeof d ? (d = new h(d), F[a] = d, d) : G[a] && b !== t && (c = t(G[a], t)) ? (d = F[a] = new h, d._set(c), d.name = H[a], d) : null } function u() { var a, b = []; for (a in H) H.hasOwnProperty(a) && (F[a] || F[G[a]]) && H[a] && b.push(H[a]); return b.sort() } function v(a) { var b, c, d, e; for ("string" == typeof a && (a = [a]), b = 0; b < a.length; b++) c = a[b].split("|"), d = r(c[0]), e = r(c[1]), G[d] = e, H[d] = c[0], G[e] = d, H[e] = c[1] } function w(a) { s(a.zones), v(a.links), A.dataVersion = a.version } function x(a) { return x.didShowError || (x.didShowError = !0, z("moment.tz.zoneExists('" + a + "') has been deprecated in favor of !moment.tz.zone('" + a + "')")), !!t(a) } function y(a) { return !(!a._a || void 0 !== a._tzm) } function z(a) { "undefined" != typeof console && "function" == typeof console.error && console.error(a) } function A(b) { var c = Array.prototype.slice.call(arguments, 0, -1), d = arguments[arguments.length - 1], e = t(d), f = a.utc.apply(null, c); return e && !a.isMoment(b) && y(f) && f.add(e.parse(f), "minutes"), f.tz(d), f } function B(a) { return function () { return this._z ? this._z.abbr(this) : a.call(this) } } function C(a) { return function () { return this._z = null, a.apply(this, arguments) } } if (void 0 !== a.tz) return z("Moment Timezone " + a.tz.version + " was already loaded " + (a.tz.dataVersion ? "with data from " : "without any data") + a.tz.dataVersion), a; var D, E = "0.5.4", F = {}, G = {}, H = {}, I = {}, J = a.version.split("."), K = +J[0], L = +J[1]; (2 > K || 2 === K && 6 > L) && z("Moment Timezone requires Moment.js >= 2.6.0. You are using Moment.js " + a.version + ". See momentjs.com"), h.prototype = { _set: function (a) { this.name = a.name, this.abbrs = a.abbrs, this.untils = a.untils, this.offsets = a.offsets, this.population = a.population }, _index: function (a) { var b, c = +a, d = this.untils; for (b = 0; b < d.length; b++) if (c < d[b]) return b }, parse: function (a) { var b, c, d, e, f = +a, g = this.offsets, h = this.untils, i = h.length - 1; for (e = 0; i > e; e++) if (b = g[e], c = g[e + 1], d = g[e ? e - 1 : e], c > b && A.moveAmbiguousForward ? b = c : b > d && A.moveInvalidForward && (b = d), f < h[e] - 6e4 * b) return g[e]; return g[i] }, abbr: function (a) { return this.abbrs[this._index(a)] }, offset: function (a) { return this.offsets[this._index(a)] } }, j.prototype.scoreOffsetAt = function (a) { this.offsetScore += Math.abs(this.zone.offset(a.at) - a.offset), this.zone.abbr(a.at).replace(/[^A-Z]/g, "") !== a.abbr && this.abbrScore++ }, A.version = E, A.dataVersion = "", A._zones = F, A._links = G, A._names = H, A.add = s, A.link = v, A.load = w, A.zone = t, A.zoneExists = x, A.guess = q, A.names = u, A.Zone = h, A.unpack = g, A.unpackBase60 = c, A.needsOffset = y, A.moveInvalidForward = !0, A.moveAmbiguousForward = !1; var M = a.fn; a.tz = A, a.defaultZone = null, a.updateOffset = function (b, c) { var d, e = a.defaultZone; void 0 === b._z && (e && y(b) && !b._isUTC && (b._d = a.utc(b._a)._d, b.utc().add(e.parse(b), "minutes")), b._z = e), b._z && (d = b._z.offset(b), Math.abs(d) < 16 && (d /= 60), void 0 !== b.utcOffset ? b.utcOffset(-d, c) : b.zone(d, c)) }, M.tz = function (b) { return b ? (this._z = t(b), this._z ? a.updateOffset(this) : z("Moment Timezone has no data for " + b + ". See http://momentjs.com/timezone/docs/#/data-loading/."), this) : this._z ? this._z.name : void 0 }, M.zoneName = B(M.zoneName), M.zoneAbbr = B(M.zoneAbbr), M.utc = C(M.utc), a.tz.setDefault = function (b) { return (2 > K || 2 === K && 9 > L) && z("Moment Timezone setDefault() requires Moment.js >= 2.9.0. You are using Moment.js " + a.version + "."), a.defaultZone = b ? t(b) : null, a }; var N = a.momentProperties; return "[object Array]" === Object.prototype.toString.call(N) ? (N.push("_z"), N.push("_a")) : N && (N._z = null), a });

moment.tz.add('Asia/Singapore|SGT|-80|0||56e5');
moment.tz.setDefault("Asia/Singapore");

var __hideLoadingWindowsOnce = false;

function GetUtcTime() {
    return moment.utc().toJSON();
}

var portalConstants =
    {
        afSalt: 'r62p+4ipqgravjbwmsrkov4z0auwtmmadtrxjzqiwxq=',
        Ascending: 'Ascending',
        Descending: 'Descending',
        thaiYearDiff: 543
    };

function RandomString(len, charSet) {
    charSet = charSet || 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var randomString = '';
    for (var i = 0; i < len; i++) {
        var randomPoz = Math.floor(Math.random() * charSet.length);
        randomString += charSet.substring(randomPoz, randomPoz + 1);
    }
    return randomString;
};
function toTitleCase(str) {
    return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
};
function IsLeapYear(utc) {
    var y = utc.getFullYear();
    return !(y % 4) && (y % 100) || !(y % 400) ? true : false;
};
function DateAddDays(dtObj, numDays) {
    var dtTemp = moment(dtObj);
    dtTemp.add(numDays, 'days');
    return dtTemp.toDate();
};
function DateAddMonths(dtObj, months) {
    var dtTemp = moment(dtObj);
    dtTemp.add(months, 'months');
    return dtTemp.toDate();
};
function getParameterByName(name, url) {
    if (!url || url == null) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function angularRedirect(win, url) {
    togglerLoadingOverlay(true);
    win.location.href = url;
}

var hideLoadingTimeout = null;

function togglerLoadingOverlay(show) {
    if (show) {

        if (hideLoadingTimeout != null) {
            window.clearTimeout(hideLoadingTimeout);
            hideLoadingTimeout = null;
        }
        console.log(new Date());
        console.log(new Date().getMilliseconds());
        jQuery(".bgloader").fadeIn('slow');
    }
    else {

        if (hideLoadingTimeout != null) {
            window.clearTimeout(hideLoadingTimeout);
            hideLoadingTimeout = null;
        }

        hideLoadingTimeout = setTimeout(function () {
            jQuery(".bgloader").fadeOut('slow');
        }, 150);
    }
}

function showAlertMessage(title, text) {
    //TH-75
    document.getElementById("lbalert_title").innerHTML = title;
    document.getElementById("lbalert_contents").innerHTML = text;
    document.getElementById("lbalert").style.display = "block";
}

function validateEmail(email) {
    //var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var re = /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/i;
    return re.test(email);
}

function isMobileBrowser()
{
    var viewportwidth;
    var viewportheight;

    // the more standards compliant browsers (mozilla/netscape/opera/IE7) use window.innerWidth and window.innerHeight

    if (typeof window.innerWidth != 'undefined') {
        viewportwidth = window.innerWidth,
        viewportheight = window.innerHeight
    }

        // IE6 in standards compliant mode (i.e. with a valid doctype as the first line in the document)

    else if (typeof document.documentElement != 'undefined'
        && typeof document.documentElement.clientWidth !=
        'undefined' && document.documentElement.clientWidth != 0) {
        viewportwidth = document.documentElement.clientWidth,
        viewportheight = document.documentElement.clientHeight
    }

        // older versions of IE

    else {
        viewportwidth = document.getElementsByTagName('body')[0].clientWidth,
        viewportheight = document.getElementsByTagName('body')[0].clientHeight
    }

    if(viewportwidth < 1061)
    {
        return true;
    }

    return false;
}

function __hightlightFormError(form, subFormName, setFocus) {
    
    if (form) {
        form.$submitted = true;
        var fristError = '';
        var firstErrorField = null;

        angular.forEach(form.$error, function (fields) {
            angular.forEach(fields, function (field) {
                console.log(field);

                if (subFormName && subFormName != null) {
                    if (field.$name == subFormName) {
                        angular.forEach(field.$error, function (sfields) {
                            angular.forEach(sfields, function (sfield) {
                                sfield.$setDirty();
                                sfield.$setTouched();
                                if (fristError == '') {
                                    fristError = sfield.$name;
                                    firstErrorField = sfield;
                                }
                            });
                        });
                    }
                    else {
                        field.$setDirty();
                        field.$setTouched();

                        if (fristError == '') {
                            fristError = field.$name;
                            firstErrorField = field;
                        }
                    }
                }
                else {
                    field.$setDirty();
                    field.$setTouched();

                    if (fristError == '') {
                        fristError = field.$name;
                        firstErrorField = field;
                    }
                }
            });
        });

        //if (setFocus && fristError != '') {
        //    var invalid_elements = $('.ng-invalid[name=' + fristError + ']');
        //    //console.log(invalid_elements);
        //    if (invalid_elements && invalid_elements.length > 0) {
        //        var firstError = invalid_elements[0];
        //        setTimeout(function () { try { jQuery('html, body').animate({ scrollTop: jQuery(firstError).offset().top - 100 }, 'slow') } catch (e) { } }, 100);
        //    }
        //}
    }
}

(function (angular) {
    'use strict';

    angular.module('easyCompare', [
      'ngCookies',
      'ngMessages'
    ])

    angular.module('easyCompare')
        .run(['$rootScope', '$cookies', '$timeout', '$http', function ($rootScope, $cookies, $timeout, $http) {
          
        }])
        .constant('motorCarConstants', {
            maxNumOfPlansForCompare: 4,
            maxNumOfPlansForCompareMobile: 2, 
            thaiYearDiff: portalConstants.thaiYearDiff,
            usage: {
                personal: 'personal',
                commercial: 'commercial'
            },
        })
        .filter('TrustedUrl', ['$sce', function ($sce) {
            return function (url) {
                return $sce.trustAsResourceUrl(url);
            };
        }])
         .filter('TrustedHtml', ['$sce', function ($sce) {
             return function (html) {
                 //var decodedHtml = decodeHtml(html);
                 return $sce.trustAsHtml(html);
             };
         }])
        .filter('EmptyAsDash', function () {
            return function (input) {
                if (!angular.isDefined(input) || input == '' || input == null) {
                    return '-';
                }
                return input;
            }
        })
        .filter('ProposalNo', function () {
            return function (input) {
                if (!angular.isDefined(input) || input == '' || input == null) {
                    return '';
                }

                var result = input;

                var index = input.lastIndexOf("/");

                if (index > 6) {
                    result = input.substr(0, index);
                }

                index = input.lastIndexOf("-");

                if (index > 6) {
                    result = input.substr(0, index);
                }

                return result;
            }
        })
        .filter('FileName', function () {
            return function (input) {
                if (!angular.isDefined(input) || input == '' || input == null) {
                    return '';
                }

                var lastDotPosition = input.lastIndexOf(".");
                if (lastDotPosition === -1) return input;
                else return input.substr(0, lastDotPosition);
            }
        })
        .filter('PolicyNo', function () {
            return function (input) {
                if (!angular.isDefined(input) || input == '' || input == null) {
                    return '';
                }

                var result = input;

                var index = input.lastIndexOf("/");

                if (index > 6) {
                    result = input.substr(0, index);
                }

                index = input.lastIndexOf("-");

                if (index > 6) {
                    result = input.substr(0, index);
                }

                return result;
            }
        })
        .filter('CardNumber', function () {
            return function (input) {
                if (!angular.isDefined(input) || input == '' || input == null) {
                    return '';
                }
                return formatccNumber(input);
            }
        })
        .directive('ncgRequestVerificationToken', ['$http', '$cookies', function ($http, $cookies) {
            return function (scope, element, attrs) {
                $cookies.put("ncg-request-verification-token", attrs.ncgRequestVerificationToken || "", { path: '/' });
                //$http.defaults.headers.common['RequestVerificationToken'] = attrs.ncgRequestVerificationToken || "";
            };
        }])
        .directive('backImg', function () {
            return {
                link: function (scope, element, attrs) {
                    attrs.$observe("backImg", function (src) {
                        element.css({
                            "background-image": "url('" + src + "')"
                        });
                    });
                }
            };
        })
        .directive('emailaddress', function () {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, elm, attr, ctrl) {

                    if (!ctrl) {
                        return;
                    }

                    attr.$observe('emailaddress', function () {
                        ctrl.$validate();
                    });

                    ctrl.$validators.emailaddress = function (modelValue, viewValue) {

                        // var value = modelValue || viewValue;

                        //console.log(modelValue);
                        return validateEmail(viewValue);
                    };
                }
            };
        })
        .directive('cmindate', ['$parse', function ($parse) {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, elm, attr, ctrl) {

                    attr.$observe('cmindate', function () {
                        ctrl.$validate();
                    });

                    scope.$watch(getMinDate, function () {
                        ctrl.$validate();
                    });

                    ctrl.$validators.cmindate = function (modelValue) {
                        var dtMin = getMinDate();
                        //console.log(dtMin);
                        //console.log(modelValue);
                        return modelValue >= dtMin;
                    };

                    function getMinDate() {
                        return $parse(attr.cmindate)(scope);
                    }
                }
            };
        }])
        .directive('cmaxdate', ['$parse', function ($parse) {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, elm, attr, ctrl) {

                    attr.$observe('cmaxdate', function () {
                        ctrl.$validate();
                    });

                    scope.$watch(getMaxDate, function () {
                        ctrl.$validate();
                    });

                    ctrl.$validators.cmaxdate = function (modelValue) {
                        var dtMax = getMaxDate();
                        //console.log(dtMax);
                        //console.log(modelValue);
                        return modelValue <= dtMax;
                    };

                    function getMaxDate() {
                        return $parse(attr.cmaxdate)(scope);
                    }
                }
            };
        }])
        .directive('checkrequired', function () {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, elm, attr, ctrl) {

                    attr.$observe('checkrequired', function () {
                        ctrl.$validate();
                    });

                    ctrl.$validators.checkrequired = function (modelValue, viewValue) {
                        var value = modelValue || viewValue;
                        var match = scope.$eval(attr.ngTrueValue) || true;
                        return value && match === value;
                    };
                }
            }
        })
        .directive('numbersonly', function () {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, element, attr, ngModelCtrl) {
                    function fromUser(text) {
                        if (text) {
                            var transformedInput = text.replace(/[^0-9]/g, '');

                            if (transformedInput !== text) {
                                ngModelCtrl.$setViewValue(transformedInput);
                                ngModelCtrl.$render();
                            }
                            return transformedInput;
                        }
                        return undefined;
                    }
                    ngModelCtrl.$parsers.push(fromUser);
                }
            };
        })
        .directive('match', ['$parse', function ($parse) {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, elm, attr, ctrl) {

                    if (!ctrl) return;

                    scope.$watch(getMatchValue, function () {
                        ctrl.$validate();
                    });

                    attr.$observe('match', function () {
                        ctrl.$validate();
                    });

                    ctrl.$validators.match = function (modelValue, viewValue) {
                        //console.log($parse(attr.match));
                        var matchValue = getMatchValue();

                        //console.log(elm);

                        var value = viewValue;

                        if (matchValue == null || value == null || !angular.isDefined(value) || !angular.isDefined(matchValue)) {
                            return true;
                        }

                        return matchValue.toLowerCase() == value.toLowerCase();
                    };

                    function getMatchValue() {
                        return $parse(attr.match)(scope);
                    }
                }
            };
        }])
        .directive('matchcase', ['$parse', function ($parse) {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, elm, attr, ctrl) {

                    scope.$watch(getMatchCaseValue, function () {
                        ctrl.$validate();
                    });

                    attr.$observe('matchcase', function () {
                        ctrl.$validate();
                    });

                    ctrl.$validators.matchcase = function (modelValue) {
                        //console.log($parse(attr.match));
                        return getMatchCaseValue() == modelValue;

                    };

                    function getMatchCaseValue() {
                        return $parse(attr.matchcase)(scope);
                    }
                }
            };
        }])
        .directive('capitalize', function () {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, element, attrs, modelCtrl) {
                    var capitalize = function (inputValue) {
                        if (inputValue == undefined) inputValue = '';
                        var capitalized = inputValue.toUpperCase();
                        if (capitalized !== inputValue) {
                            modelCtrl.$setViewValue(capitalized);
                            modelCtrl.$render();
                        }
                        return capitalized;
                    }
                    modelCtrl.$parsers.push(capitalize);
                    capitalize(scope[attrs.ngModel]); // capitalize initial value
                }
            };
        })
        .filter('ShortDate', [
                    '$filter', function ($filter) {
                        return function (theDate) {
                            if (!angular.isDefined(theDate) || theDate == null) {
                                return "";
                            }
                            return $filter('date')(theDate, 'dd/MM/yyyy', '+0800');
                        }
                    }
        ])
        .filter('LongDate', [
                    '$filter', function ($filter) {
                        return function (theDate) {
                            if (!angular.isDefined(theDate) || theDate == null) {
                                return "";
                            }
                            return $filter('date')(theDate, 'dd/MM/yyyy HH:mm:ss', '+0800');
                        }
                    }
        ])
        .filter('Amount', [
                    '$filter', function ($filter) {
                        return function (amt) {
                            return $filter('currency')(amt, '', 0);
                        }
                    }
        ])
         .filter('SumInsured', [
                    '$filter', function ($filter) {
                        return function (amt) {
                            return $filter('currency')(amt, '', 0);
                        }
                    }
         ])
        .factory('appService', [
                     '$http', function ($http) {
                         var appServiceFactory = {};
                         var serviceData = {};

                         var _setServiceData = function (name, val) {
                             //localStorageService.set(name, val);
                             serviceData[name] = val;
                         }

                         var _getServiceData = function (name) {
                             //return localStorageService.get(name)
                             return serviceData[name];
                         }

                         var _execService = function (url, requestData) {
                             return $http.post(url, requestData).success(function (results) {
                                 return results;
                             });
                         }

                         appServiceFactory.PostData = function (requestData, actionUrl) {
                             return _execService(actionUrl, requestData);
                         };

                         appServiceFactory.setServiceData = _setServiceData;
                         appServiceFactory.getServiceData = _getServiceData;

                         return appServiceFactory;
                     }
        ])
        .factory('authInterceptorService', [
                     '$q', '$injector', '$location', '$cookies', '$rootScope', '$timeout', '$window', function ($q, $injector, $location, $cookies, $rootScope, $timeout, $window) {
                         var authInterceptorServiceFactory = {};

                         var _request = function (config) {
                             config.headers = config.headers || {};
                             //console.log(config);

                             if (__hideLoadingWindowsOnce) {
                                 console.log("hide loading once....");
                                 __hideLoadingWindowsOnce = false;
                             }
                             else {
                                 console.log("show loading....");
                                 //BD_utility.global_loader.show();
                                 togglerLoadingOverlay(true);
                             }

                             var ts = GetUtcTime() + '';

                             config.headers['RequestVerificationTokenTs'] = ts;
                             config.headers['RequestVerificationTokenHash'] = _getChecksum(ts);

                             return config;
                         };

                         var _getChecksum = function (ts) {
                             var h = $cookies.get("ncg-request-verification-token");

                             if (h != null) {
                                 var ha = h.split(':');
                                 var word = portalConstants.afSalt + '|' + ts;
                                 return CryptoJS.HmacSHA256(word, ha[2]).toString(CryptoJS.enc.Base64)
                             }

                             return null;
                         }

                         var _responseError = function (rejection) {
                             console.log("hide loading....");
                             //BD_utility.global_loader.hide();
                             togglerLoadingOverlay(false);
                             //console.log(rejection);
                             return $q.reject(rejection);
                         };

                         var _responseSuccess = function (response) {
                             console.log("hide loading....");
                             //BD_utility.global_loader.hide();
                             togglerLoadingOverlay(false);
                             //console.log("authInterceptorService");
                             $rootScope["hasFormError"] = false;
                             //console.log(response);
                             if (response.data.status == 'F') {
                                 //console.log(response.data);

                                 //if (!isOverlasyPopupOpen()) {
                                 //    $rootScope["errorMessages"] = [];
                                 //    $rootScope["errorMessages"].push({ msg: response.data.errorMessage, code: response.data.errorCode });
                                 //    $rootScope["hasFormError"] = true;

                                 //    var pm = getParameterByName("pm");
                                 //    if (pm && pm != '' && pm != null) {
                                 //        $timeout(function () { try { jQuery('html, body').animate({ scrollTop: jQuery('#qbMainFromError').offset().top - 150 }, 'slow') } catch (e) { } }, 100);
                                 //    }
                                 //}
                                 alert(response.data.errorCode + ': ' + response.data.errorMessage);
                             }
                             return response || $q.when(response);
                         };

                         authInterceptorServiceFactory.request = _request;
                         authInterceptorServiceFactory.response = _responseSuccess;
                         authInterceptorServiceFactory.responseError = _responseError;

                         return authInterceptorServiceFactory;
                     }
        ])
        .config([
                    '$httpProvider', function ($httpProvider) {
                        $httpProvider.interceptors.push('authInterceptorService');
                    }
        ]);

})(window.angular);