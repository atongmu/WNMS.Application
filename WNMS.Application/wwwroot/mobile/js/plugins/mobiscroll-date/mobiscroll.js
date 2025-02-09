! function(e, t) {
	"function" == typeof define && define.amd ? define(["jquery"], t) : "object" == typeof exports ? module.exports = t(
		require("jquery")) : e.mobiscroll = t(e.jQuery)
}(this, function(e) {
	var t = t || {};
	! function(e, a, s) {
		function n(e) {
			return "function" == typeof e
		}

		function i(e) {
			return "object" == typeof e
		}

		function r(e) {
			return "number" == typeof e.length
		}

		function o(e) {
			return e.replace(/-+(.)?/g, function(e, t) {
				return t ? t.toUpperCase() : ""
			})
		}

		function l(e, t, a) {
			for (var n in t) a && (b.isPlainObject(t[n]) || b.isArray(t[n])) ? ((b.isPlainObject(t[n]) && !b
				.isPlainObject(e[n]) || b.isArray(t[n]) && !b.isArray(e[n])) && (e[n] = {}), l(e[n], t[n],
				a)) : t[n] !== s && (e[n] = t[n])
		}

		function c(e) {
			return e.replace(/::/g, "/").replace(/([A-Z]+)([A-Z][a-z])/g, "$1_$2").replace(/([a-z\d])([A-Z])/g,
				"$1_$2").replace(/_/g, "-").toLowerCase()
		}

		function m(e, t) {
			return "number" != typeof t || d[c(e)] ? t : t + "px"
		}
		var d = {
				"column-count": 1,
				columns: 1,
				"font-weight": 1,
				"line-height": 1,
				opacity: 1,
				"z-index": 1,
				zoom: 1
			},
			u = {
				readonly: "readOnly"
			},
			h = [],
			f = Array.prototype.slice,
			p = function() {
				var t = function(e) {
						var t = this,
							a = 0;
						for (a = 0; a < e.length; a++) t[a] = e[a];
						return t.length = e.length, l(this)
					},
					l = function(s, i) {
						var r = [],
							o = 0;
						if (s && !i && s instanceof t) return s;
						if (n(s)) return l(a).ready(s);
						if (s)
							if ("string" == typeof s) {
								var c, m, d;
								if (s = d = s.trim(), d.indexOf("<") >= 0 && d.indexOf(">") >= 0) {
									var u = "div";
									for (0 === d.indexOf("<li") && (u = "ul"), 0 === d.indexOf("<tr") && (u =
											"tbody"), (0 === d.indexOf("<td") || 0 === d.indexOf("<th")) && (u =
											"tr"), 0 === d.indexOf("<tbody") && (u = "table"), 0 === d.indexOf(
											"<option") && (u = "select"), m = a.createElement(u), m.innerHTML = d,
										o = 0; o < m.childNodes.length; o++) r.push(m.childNodes[o])
								} else
									for (i || "#" !== s[0] || s.match(/[ .<>:~]/) ? (i instanceof t && (i = i[0]),
											c = (i || a).querySelectorAll(s)) : c = [a.getElementById(s.split("#")[
											1])], o = 0; o < c.length; o++) c[o] && r.push(c[o])
							} else if (s.nodeType || s === e || s === a) r.push(s);
						else if (s.length > 0 && s[0].nodeType)
							for (o = 0; o < s.length; o++) r.push(s[o]);
						else l.isArray(s) && (r = s);
						return new t(r)
					};
				return t.prototype = {
					ready: function(e) {
						return /complete|loaded|interactive/.test(a.readyState) && a.body ? e(l) : a
							.addEventListener("DOMContentLoaded", function() {
								e(l)
							}, !1), this
					},
					concat: h.concat,
					empty: function() {
						return this.each(function() {
							this.innerHTML = ""
						})
					},
					map: function(e) {
						return l(l.map(this, function(t, a) {
							return e.call(t, a, t)
						}))
					},
					slice: function() {
						return l(f.apply(this, arguments))
					},
					addClass: function(e) {
						if ("undefined" == typeof e) return this;
						for (var t = e.split(" "), a = 0; a < t.length; a++)
							for (var s = 0; s < this.length; s++) "undefined" != typeof this[s].classList &&
								"" !== t[a] && this[s].classList.add(t[a]);
						return this
					},
					removeClass: function(e) {
						for (var t = e.split(" "), a = 0; a < t.length; a++)
							for (var s = 0; s < this.length; s++) "undefined" != typeof this[s].classList &&
								"" !== t[a] && this[s].classList.remove(t[a]);
						return this
					},
					hasClass: function(e) {
						return this[0] ? this[0].classList.contains(e) : !1
					},
					toggleClass: function(e) {
						for (var t = e.split(" "), a = 0; a < t.length; a++)
							for (var s = 0; s < this.length; s++) "undefined" != typeof this[s].classList &&
								this[s].classList.toggle(t[a]);
						return this
					},
					closest: function(e, t) {
						var a = this[0],
							s = !1;
						for (i(e) && (s = l(e)); a && !(s ? s.indexOf(a) >= 0 : l.matches(a, e));) a = a !==
							t && a.nodeType !== a.DOCUMENT_NODE && a.parentNode;
						return l(a)
					},
					attr: function(e, t) {
						var a;
						if (1 === arguments.length && "string" == typeof e && this.length) return a = this[
							0].getAttribute(e), this[0] && (a || "" === a) ? a : s;
						for (var n = 0; n < this.length; n++)
							if (2 === arguments.length) this[n].setAttribute(e, t);
							else
								for (var i in e) this[n][i] = e[i], this[n].setAttribute(i, e[i]);
						return this
					},
					removeAttr: function(e) {
						for (var t = 0; t < this.length; t++) this[t].removeAttribute(e);
						return this
					},
					prop: function(e, t) {
						if (e = u[e] || e, 1 === arguments.length && "string" == typeof e) return this[0] ?
							this[0][e] : s;
						for (var a = 0; a < this.length; a++) this[a][e] = t;
						return this
					},
					val: function(e) {
						if ("undefined" == typeof e) return this.length && this[0].multiple ? l.map(this
							.find("option:checked"),
							function(e) {
								return e.value
							}) : this[0] ? this[0].value : s;
						if (this.length && this[0].multiple) l.each(this[0].options, function() {
							this.selected = -1 != e.indexOf(this.value)
						});
						else
							for (var t = 0; t < this.length; t++) this[t].value = e;
						return this
					},
					on: function(e, t, a, s) {
						function i(e) {
							var s, n, i = e.target;
							if (l(i).is(t)) a.call(i, e);
							else
								for (n = l(i).parents(), s = 0; s < n.length; s++) l(n[s]).is(t) && a.call(
									n[s], e)
						}

						function r(e, t, a, s) {
							var n = t.split(".");
							e.DomNameSpaces || (e.DomNameSpaces = []), e.DomNameSpaces.push({
								namespace: n[1],
								event: n[0],
								listener: a,
								capture: s
							}), e.addEventListener(n[0], a, s)
						}
						var o, c, m = e.split(" ");
						for (o = 0; o < this.length; o++)
							if (n(t) || t === !1)
								for (n(t) && (s = a || !1, a = t), c = 0; c < m.length; c++) - 1 != m[c]
									.indexOf(".") ? r(this[o], m[c], a, s) : this[o].addEventListener(m[c],
										a, s);
							else
								for (c = 0; c < m.length; c++) this[o].DomLiveListeners || (this[o]
										.DomLiveListeners = []), this[o].DomLiveListeners.push({
										listener: a,
										liveListener: i
									}), -1 != m[c].indexOf(".") ? r(this[o], m[c], i, s) : this[o]
									.addEventListener(m[c], i, s);
						return this
					},
					off: function(e, t, a, s) {
						function i(e) {
							var t, a, s, n = e.split("."),
								i = n[0],
								r = n[1];
							for (t = 0; t < m.length; ++t)
								if (m[t].DomNameSpaces) {
									for (a = 0; a < m[t].DomNameSpaces.length; ++a) s = m[t].DomNameSpaces[
										a], s.namespace != r || s.event != i && i || (m[t]
										.removeEventListener(s.event, s.listener, s.capture), s
										.removed = !0);
									for (a = m[t].DomNameSpaces.length - 1; a >= 0; --a) m[t].DomNameSpaces[
										a].removed && m[t].DomNameSpaces.splice(a, 1)
								}
						}
						var r, o, l, c, m = this;
						for (r = e.split(" "), o = 0; o < r.length; o++)
							for (l = 0; l < this.length; l++)
								if (n(t) || t === !1) n(t) && (s = a || !1, a = t), 0 === r[o].indexOf(
									".") ? i(r[o].substr(1), a, s) : this[l].removeEventListener(r[o], a,
									s);
								else {
									if (this[l].DomLiveListeners)
										for (c = 0; c < this[l].DomLiveListeners.length; c++) this[l]
											.DomLiveListeners[c].listener === a && this[l]
											.removeEventListener(r[o], this[l].DomLiveListeners[c]
												.liveListener, s);
									this[l].DomNameSpaces && this[l].DomNameSpaces.length && r[o] && i(r[o])
								} return this
					},
					trigger: function(e, t) {
						for (var s = e.split(" "), n = 0; n < s.length; n++)
							for (var i = 0; i < this.length; i++) {
								var r;
								try {
									r = new CustomEvent(s[n], {
										detail: t,
										bubbles: !0,
										cancelable: !0
									})
								} catch (i) {
									r = a.createEvent("Event"), r.initEvent(s[n], !0, !0), r.detail = t
								}
								this[i].dispatchEvent(r)
							}
						return this
					},
					width: function(t) {
						return t !== s ? this.css("width", t) : this[0] === e ? e.innerWidth : this[0] ===
							a ? a.documentElement.scrollWidth : this.length > 0 ? parseFloat(this.css(
								"width")) : null
					},
					height: function(t) {
						if (t !== s) return this.css("height", t);
						if (this[0] === e) return e.innerHeight;
						if (this[0] === a) {
							var n = a.body,
								i = a.documentElement;
							return Math.max(n.scrollHeight, n.offsetHeight, i.clientHeight, i.scrollHeight,
								i.offsetHeight)
						}
						return this.length > 0 ? parseFloat(this.css("height")) : null
					},
					innerWidth: function() {
						var e = this;
						if (this.length > 0) {
							if (this[0].innerWidth) return this[0].innerWidth;
							var t = this[0].offsetWidth,
								a = ["left", "right"];
							return a.forEach(function(a) {
								t -= parseInt(e.css(o("border-" + a + "-width")) || 0, 10)
							}), t
						}
					},
					innerHeight: function() {
						var e = this;
						if (this.length > 0) {
							if (this[0].innerHeight) return this[0].innerHeight;
							var t = this[0].offsetHeight,
								a = ["top", "bottom"];
							return a.forEach(function(a) {
								t -= parseInt(e.css(o("border-" + a + "-width")) || 0, 10)
							}), t
						}
					},
					offset: function() {
						if (this.length > 0) {
							var t = this[0],
								s = t.getBoundingClientRect(),
								n = a.body,
								i = t.clientTop || n.clientTop || 0,
								r = t.clientLeft || n.clientLeft || 0,
								o = e.pageYOffset || t.scrollTop,
								l = e.pageXOffset || t.scrollLeft;
							return {
								top: s.top + o - i,
								left: s.left + l - r
							}
						}
					},
					hide: function() {
						for (var e = 0; e < this.length; e++) this[e].style.display = "none";
						return this
					},
					show: function() {
						for (var e = 0; e < this.length; e++) "none" == this[e].style.display && (this[e]
								.style.display = ""), "none" == getComputedStyle(this[e], "")
							.getPropertyValue("display") && (this[e].style.display = "block");
						return this
					},
					clone: function() {
						return this.map(function() {
							return this.cloneNode(!0)
						})
					},
					styles: function() {
						return this[0] ? e.getComputedStyle(this[0], null) : s
					},
					css: function(e, t) {
						var a, s, n = this[0],
							i = "";
						if (arguments.length < 2) {
							if (!n) return;
							if ("string" == typeof e) return n.style[e] || getComputedStyle(n, "")
								.getPropertyValue(e)
						}
						if ("string" == typeof e) t || 0 === t ? i = c(e) + ":" + m(e, t) : this.each(
							function() {
								this.style.removeProperty(c(e))
							});
						else
							for (s in e)
								if (e[s] || 0 === e[s]) i += c(s) + ":" + m(s, e[s]) + ";";
								else
									for (a = 0; a < this.length; a++) this[a].style.removeProperty(c(s));
						return this.each(function() {
							this.style.cssText += ";" + i
						})
					},
					each: function(e) {
						for (var t = 0; t < this.length && e.apply(this[t], [t, this[t]]) !== !1; t++);
						return this
					},
					filter: function(e) {
						for (var a = [], s = 0; s < this.length; s++) n(e) ? e.call(this[s], s, this[s]) &&
							a.push(this[s]) : l.matches(this[s], e) && a.push(this[s]);
						return new t(a)
					},
					html: function(e) {
						if ("undefined" == typeof e) return this[0] ? this[0].innerHTML : s;
						this.empty();
						for (var t = 0; t < this.length; t++) this[t].innerHTML = e;
						return this
					},
					text: function(e) {
						if ("undefined" == typeof e) return this[0] ? this[0].textContent.trim() : null;
						for (var t = 0; t < this.length; t++) this[t].textContent = e;
						return this
					},
					is: function(e) {
						return this.length > 0 && l.matches(this[0], e)
					},
					not: function(e) {
						var t = [];
						if (n(e) && e.call !== s) this.each(function(a) {
							e.call(this, a) || t.push(this)
						});
						else {
							var a = "string" == typeof e ? this.filter(e) : r(e) && n(e.item) ? f.call(e) :
								l(e);
							i(a) && (a = l.map(a, function(e) {
								return e
							})), this.each(function(e, s) {
								a.indexOf(s) < 0 && t.push(s)
							})
						}
						return l(t)
					},
					indexOf: function(e) {
						for (var t = 0; t < this.length; t++)
							if (this[t] === e) return t
					},
					index: function(e) {
						return e ? this.indexOf(l(e)[0]) : this.parent().children().indexOf(this[0])
					},
					get: function(e) {
						return e === s ? f.call(this) : this[e >= 0 ? e : e + this.length]
					},
					eq: function(e) {
						if ("undefined" == typeof e) return this;
						var a, s = this.length;
						return e > s - 1 ? new t([]) : 0 > e ? (a = s + e, new t(0 > a ? [] : [this[a]])) :
							new t([this[e]])
					},
					append: function(e) {
						var s, n;
						for (s = 0; s < this.length; s++)
							if ("string" == typeof e) {
								var i = a.createElement("div");
								for (i.innerHTML = e; i.firstChild;) this[s].appendChild(i.firstChild)
							} else if (e instanceof t)
							for (n = 0; n < e.length; n++) this[s].appendChild(e[n]);
						else this[s].appendChild(e);
						return this
					},
					appendTo: function(e) {
						return l(e).append(this), this
					},
					prepend: function(e) {
						var s, n;
						for (s = 0; s < this.length; s++)
							if ("string" == typeof e) {
								var i = a.createElement("div");
								for (i.innerHTML = e, n = i.childNodes.length - 1; n >= 0; n--) this[s]
									.insertBefore(i.childNodes[n], this[s].childNodes[0])
							} else if (e instanceof t)
							for (n = 0; n < e.length; n++) this[s].insertBefore(e[n], this[s].childNodes[
							0]);
						else this[s].insertBefore(e, this[s].childNodes[0]);
						return this
					},
					prependTo: function(e) {
						return l(e).prepend(this), this
					},
					insertBefore: function(e) {
						for (var t = l(e), a = 0; a < this.length; a++)
							if (1 === t.length) t[0].parentNode.insertBefore(this[a], t[0]);
							else if (t.length > 1)
							for (var s = 0; s < t.length; s++) t[s].parentNode.insertBefore(this[a]
								.cloneNode(!0), t[s]);
						return this
					},
					insertAfter: function(e) {
						for (var t = l(e), a = 0; a < this.length; a++)
							if (1 === t.length) t[0].parentNode.insertBefore(this[a], t[0].nextSibling);
							else if (t.length > 1)
							for (var s = 0; s < t.length; s++) t[s].parentNode.insertBefore(this[a]
								.cloneNode(!0), t[s].nextSibling);
						return this
					},
					next: function(e) {
						return new t(this.length > 0 ? e ? this[0].nextElementSibling && l(this[0]
								.nextElementSibling).is(e) ? [this[0].nextElementSibling] : [] : this[0]
							.nextElementSibling ? [this[0].nextElementSibling] : [] : [])
					},
					nextAll: function(e) {
						var a = [],
							s = this[0];
						if (!s) return new t([]);
						for (; s.nextElementSibling;) {
							var n = s.nextElementSibling;
							e ? l(n).is(e) && a.push(n) : a.push(n), s = n
						}
						return new t(a)
					},
					prev: function(e) {
						return new t(this.length > 0 ? e ? this[0].previousElementSibling && l(this[0]
								.previousElementSibling).is(e) ? [this[0].previousElementSibling] : [] :
							this[0].previousElementSibling ? [this[0].previousElementSibling] : [] : [])
					},
					prevAll: function(e) {
						var a = [],
							s = this[0];
						if (!s) return new t([]);
						for (; s.previousElementSibling;) {
							var n = s.previousElementSibling;
							e ? l(n).is(e) && a.push(n) : a.push(n), s = n
						}
						return new t(a)
					},
					parent: function(e) {
						for (var t = [], a = 0; a < this.length; a++) null !== this[a].parentNode && (e ? l(
							this[a].parentNode).is(e) && t.push(this[a].parentNode) : t.push(this[a]
							.parentNode));
						return l(l.unique(t))
					},
					parents: function(e) {
						for (var t = [], a = 0; a < this.length; a++)
							for (var s = this[a].parentNode; s;) e ? l(s).is(e) && t.push(s) : t.push(s),
								s = s.parentNode;
						return l(l.unique(t))
					},
					find: function(e) {
						for (var a = [], s = 0; s < this.length; s++)
							for (var n = this[s].querySelectorAll(e), i = 0; i < n.length; i++) a.push(n[
							i]);
						return new t(a)
					},
					children: function(e) {
						for (var a = [], s = 0; s < this.length; s++)
							for (var n = this[s].childNodes, i = 0; i < n.length; i++) e ? 1 === n[i]
								.nodeType && l(n[i]).is(e) && a.push(n[i]) : 1 === n[i].nodeType && a.push(
									n[i]);
						return new t(l.unique(a))
					},
					remove: function() {
						for (var e = 0; e < this.length; e++) this[e].parentNode && this[e].parentNode
							.removeChild(this[e]);
						return this
					},
					add: function() {
						var e, t, a = this;
						for (e = 0; e < arguments.length; e++) {
							var s = l(arguments[e]);
							for (t = 0; t < s.length; t++) a[a.length] = s[t], a.length++
						}
						return a
					},
					before: function(e) {
						return l(e).insertBefore(this), this
					},
					after: function(e) {
						return l(e).insertAfter(this), this
					},
					scrollTop: function(e) {
						if (this.length) {
							var t = "scrollTop" in this[0];
							return e === s ? t ? this[0].scrollTop : this[0].pageYOffset : this.each(t ?
								function() {
									this.scrollTop = e
								} : function() {
									this.scrollTo(this.scrollX, e)
								})
						}
					},
					scrollLeft: function(e) {
						if (this.length) {
							var t = "scrollLeft" in this[0];
							return e === s ? t ? this[0].scrollLeft : this[0].pageXOffset : this.each(t ?
								function() {
									this.scrollLeft = e
								} : function() {
									this.scrollTo(e, this.scrollY)
								})
						}
					},
					contents: function() {
						return this.map(function(e, t) {
							return f.call(t.childNodes)
						})
					},
					nextUntil: function(e) {
						for (var t = this, a = []; t.length && !t.filter(e).length;) a.push(t[0]), t = t
							.next();
						return l(a)
					},
					prevUntil: function(e) {
						for (var t = this, a = []; t.length && !l(t).filter(e).length;) a.push(t[0]), t = t
							.prev();
						return l(a)
					},
					detach: function() {
						return this.remove()
					}
				}, l.fn = t.prototype, l
			}(),
			b = p;
		t.$ = p, b.inArray = function(e, t, a) {
			return h.indexOf.call(t, e, a)
		}, b.extend = function(e) {
			var t, a = f.call(arguments, 1);
			return "boolean" == typeof e && (t = e, e = a.shift()), e = e || {}, a.forEach(function(a) {
				l(e, a, t)
			}), e
		}, b.isFunction = n, b.isArray = function(e) {
			return "[object Array]" === Object.prototype.toString.apply(e)
		}, b.isPlainObject = function(e) {
			return i(e) && null !== e && e !== e.window && Object.getPrototypeOf(e) == Object.prototype
		}, b.each = function(e, t) {
			var a, s;
			if (i(e) && t) {
				if (b.isArray(e) || e instanceof p)
					for (a = 0; a < e.length && t.call(e[a], a, e[a]) !== !1; a++);
				else
					for (s in e)
						if (e.hasOwnProperty(s) && "length" !== s && t.call(e[s], s, e[s]) === !1) break;
				return this
			}
		}, b.unique = function(e) {
			for (var t = [], a = 0; a < e.length; a++) - 1 === t.indexOf(e[a]) && t.push(e[a]);
			return t
		}, b.map = function(e, t) {
			var a, s, n, i = [];
			if (r(e))
				for (s = 0; s < e.length; s++) a = t(e[s], s), null !== a && i.push(a);
			else
				for (n in e) a = t(e[n], n), null !== a && i.push(a);
			return i.length > 0 ? b.fn.concat.apply([], i) : i
		}, b.matches = function(e, t) {
			if (!t || !e || 1 !== e.nodeType) return !1;
			var a = e.matchesSelector || e.webkitMatchesSelector || e.mozMatchesSelector || e.msMatchesSelector;
			return a.call(e, t)
		}
	}(window, document);
	var t = t || {};
	return function() {
			return function(a, s, n) {
				function i(e) {
					var t;
					for (t in e)
						if (g[e[t]] !== n) return !0;
					return !1
				}

				function r() {
					var e, t = ["Webkit", "Moz", "O", "ms"];
					for (e in t)
						if (i([t[e] + "Transform"])) return "-" + t[e].toLowerCase() + "-";
					return ""
				}

				function o(e, a, s) {
					var i = e;
					return "object" == typeof a ? e.each(function() {
						f[this.id] && f[this.id].destroy(), new t.classes[a.component || "Scroller"](
							this, a)
					}) : ("string" == typeof a && e.each(function() {
						var e, t = f[this.id];
						return t && t[a] && (e = t[a].apply(this, Array.prototype.slice.call(s, 1)),
							e !== n) ? (i = e, !1) : void 0
					}), i)
				}

				function l(e) {
					return !c.tapped || e.tap || "TEXTAREA" == e.target.nodeName && "mousedown" == e.type ?
						void 0 : (e.stopPropagation(), e.preventDefault(), !1)
				}
				var c, m, d, u = "undefined" == typeof e ? t.$ : e,
					h = +new Date,
					f = {},
					p = u.extend,
					b = navigator.userAgent,
					v = b.match(/Android|iPhone|iPad|iPod|Windows Phone|Windows|MSIE/i),
					g = s.createElement("modernizr").style,
					x = i(["perspectiveProperty", "WebkitPerspective", "MozPerspective", "OPerspective",
						"msPerspective"
					]),
					T = i(["flex", "msFlex", "WebkitBoxDirection"]),
					y = r(),
					w = y.replace(/^\-/, "").replace(/\-$/, "").replace("moz", "Moz"),
					C = [];
				/Android/i.test(v) ? (m = "android", d = navigator.userAgent.match(/Android\s+([\d\.]+)/i), d &&
						(C = d[0].replace("Android ", "").split("."))) : /iPhone|iPad|iPod/i.test(v) ? (m =
						"ios", d = navigator.userAgent.match(/OS\s+([\d\_]+)/i), d && (C = d[0].replace(/_/g,
							".").replace("OS ", "").split("."))) : /Windows Phone/i.test(v) ? m = "wp" :
					/Windows|MSIE/i.test(v) && (m = "windows"), c = t = {
						$: u,
						version: "3.0.0-beta6",
						vKMaI: 1,
						running: !0,
						util: {
							prefix: y,
							jsPrefix: w,
							has3d: x,
							hasFlex: T,
							preventClick: function() {
								c.tapped++, setTimeout(function() {
									c.tapped--
								}, 500)
							},
							testTouch: function(e, t) {
								if ("touchstart" == e.type) u(t).attr("data-touch", "1");
								else if (u(t).attr("data-touch")) return u(t).removeAttr("data-touch"), !1;
								return !0
							},
							objectToArray: function(e) {
								var t, a = [];
								for (t in e) a.push(e[t]);
								return a
							},
							arrayToObject: function(e) {
								var t, a = {};
								if (e)
									for (t = 0; t < e.length; t++) a[e[t]] = e[t];
								return a
							},
							isNumeric: function(e) {
								return e - parseFloat(e) >= 0
							},
							isString: function(e) {
								return "string" == typeof e
							},
							getCoord: function(e, t, a) {
								var s = e.originalEvent || e,
									n = (a ? "page" : "client") + t;
								return s.targetTouches && s.targetTouches[0] ? s.targetTouches[0][n] : s
									.changedTouches && s.changedTouches[0] ? s.changedTouches[0][n] : e[n]
							},
							getPosition: function(e, t) {
								var a, s, i = getComputedStyle(e[0]);
								return u.each(["t", "webkitT", "MozT", "OT", "msT"], function(e, t) {
									return i[t + "ransform"] !== n ? (a = i[t + "ransform"], !1) :
										void 0
								}), a = a.split(")")[0].split(", "), s = t ? a[13] || a[5] : a[12] || a[
									4]
							},
							constrain: function(e, t, a) {
								return Math.max(t, Math.min(e, a))
							},
							vibrate: function(e) {
								"vibrate" in navigator && navigator.vibrate(e || 50)
							},
							throttle: function(e, t) {
								var a, s;
								return t = t || 100,
									function() {
										var n = this,
											i = +new Date,
											r = arguments;
										a && a + t > i ? (clearTimeout(s), s = setTimeout(function() {
											a = i, e.apply(n, r)
										}, t)) : (a = i, e.apply(n, r))
									}
							}
						},
						tapped: 0,
						autoTheme: "mobiscroll",
						presets: {
							scroller: {},
							numpad: {},
							listview: {},
							menustrip: {}
						},
						themes: {
							form: {},
							frame: {},
							listview: {},
							menustrip: {},
							progress: {}
						},
						platform: {
							name: m,
							majorVersion: C[0],
							minorVersion: C[1]
						},
						i18n: {},
						instances: f,
						classes: {},
						components: {},
						settings: {},
						setDefaults: function(e) {
							p(this.settings, e)
						},
						presetShort: function(e, t, a) {
							c[e] = function(s, i) {
								var r, o, l = {},
									m = i || {};
								return u.extend(m, {
									preset: a === !1 ? n : e
								}), u(s).each(function() {
									f[this.id] && f[this.id].destroy(), r = new c.classes[t ||
										"Scroller"](this, m), l[this.id] = r
								}), o = Object.keys(l), 1 == o.length ? l[o[0]] : l
							}, this.components[e] = function(s) {
								return o(this, p(s, {
									component: t,
									preset: a === !1 ? n : e
								}), arguments)
							}
						}
					}, u.mobiscroll = t, u.fn.mobiscroll = function(e) {
						return p(this, t.components), o(this, e, arguments)
					}, t.classes.Base = function(e, a) {
						var s, n, i, r, o, l, c = t,
							m = c.util,
							d = m.getCoord,
							b = this;
						b.settings = {}, b._presetLoad = function() {}, b._init = function(t) {
							var m;
							for (m in b.settings) delete b.settings[m];
							i = b.settings, p(a, t), b._hasDef && (l = c.settings), p(i, b._defaults, l, a),
								b._hasTheme && (o = i.theme, "auto" != o && o || (o = c.autoTheme),
									"default" == o && (o = "mobiscroll"), a.theme = o, r = c.themes[b
										._class] ? c.themes[b._class][o] : {}), b._hasLang && (s = c.i18n[i
									.lang]), b._hasTheme && b.trigger("onThemeLoad", {
									lang: s,
									settings: a
								}), p(i, r, s, l, a), b._hasPreset && (b._presetLoad(i), n = c.presets[b
									._class][i.preset], n && (n = n.call(e, b), p(i, n, a)))
						}, b._destroy = function() {
							b && (b.trigger("onDestroy", []), delete f[e.id], b = null)
						}, b.tap = function(e, t, a, s) {
							function n(e) {
								h || (a && e.preventDefault(), h = this, c = d(e, "X"), u = d(e, "Y"), f = !
									1, p = new Date)
							}

							function r(e) {
								h && !f && (Math.abs(d(e, "X") - c) > s || Math.abs(d(e, "Y") - u) > s) && (
									f = !0)
							}

							function o(e) {
								h && ((new Date - p < 100 || !f) && (e.preventDefault(), t.call(h, e, b)),
									h = !1, m.preventClick())
							}

							function l() {
								h = !1
							}
							var c, u, h, f, p;
							s = s || 9, i.tap && e.on("touchstart.mbsc", n).on("touchcancel.mbsc", l).on(
								"touchmove.mbsc", r).on("touchend.mbsc", o), e.on("click.mbsc",
								function(e) {
									e.preventDefault(), t.call(this, e, b)
								})
						}, b.trigger = function(t, s) {
							var i, o, c, m = [l, r, n, a];
							for (o = 0; 4 > o; o++) c = m[o], c && c[t] && (i = c[t].call(e, s || {}, b));
							return i
						}, b.option = function(e, t) {
							var a = {};
							"object" == typeof e ? a = e : a[e] = t, b.init(a)
						}, b.getInst = function() {
							return b
						}, a = a || {}, u(e).addClass("mbsc-comp"), e.id || (e.id = "mobiscroll" + ++h), f[e
							.id] = b
					}, s.addEventListener && u.each(["mouseover", "mousedown", "mouseup", "click"], function(e,
						t) {
						s.addEventListener(t, l, !0)
					})
			}
		}()(window, document), window.mobiscroll = t,
		function() {
			t.i18n.ca = {
				setText: "Acceptar",
				cancelText: "Cancel·lar",
				clearText: "Esborrar",
				selectedText: "{count} seleccionat",
				selectedPluralText: "{count} seleccionats",
				dateFormat: "dd/mm/yy",
				dayNames: ["Diumenge", "Dilluns", "Dimarts", "Dimecres", "Dijous", "Divendres", "Dissabte"],
				dayNamesShort: ["Dg", "Dl", "Dt", "Dc", "Dj", "Dv", "Ds"],
				dayNamesMin: ["Dg", "Dl", "Dt", "Dc", "Dj", "Dv", "Ds"],
				dayText: "Dia",
				hourText: "Hores",
				minuteText: "Minuts",
				monthNames: ["Gener", "Febrer", "Mar&ccedil;", "Abril", "Maig", "Juny", "Juliol", "Agost",
					"Setembre", "Octubre", "Novembre", "Desembre"
				],
				monthNamesShort: ["Gen", "Feb", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Oct", "Nov",
					"Des"
				],
				monthText: "Mes",
				secText: "Segons",
				timeFormat: "HH:ii",
				yearText: "Any",
				nowText: "Ara",
				pmText: "pm",
				amText: "am",
				todayText: "Avui",
				firstDay: 1,
				dateText: "Data",
				timeText: "Temps",
				calendarText: "Calendari",
				closeText: "Tancar",
				fromText: "Iniciar",
				toText: "Final",
				wholeText: "Sencer",
				fractionText: "Fracció",
				unitText: "Unitat",
				labels: ["Anys", "Mesos", "Dies", "Hores", "Minuts", "Segons", ""],
				labelsShort: ["Anys", "Mesos", "Dies", "Hrs", "Mins", "Secs", ""],
				startText: "Iniciar",
				stopText: "Aturar",
				resetText: "Reiniciar",
				lapText: "Volta",
				hideText: "Amagar",
				backText: "Tornar",
				undoText: "Desfer",
				offText: "No",
				onText: "Si"
			}
		}(),
		function() {
			t.i18n.cs = {
				setText: "Zadej",
				cancelText: "Storno",
				clearText: "Vymazat",
				selectedText: "Označený: {count}",
				dateFormat: "dd.mm.yy",
				dayNames: ["Neděle", "Pondělí", "Úterý", "Středa", "Čtvrtek", "Pátek", "Sobota"],
				dayNamesShort: ["Ne", "Po", "Út", "St", "Čt", "Pá", "So"],
				dayNamesMin: ["N", "P", "Ú", "S", "Č", "P", "S"],
				dayText: "Den",
				hourText: "Hodiny",
				minuteText: "Minuty",
				monthNames: ["Leden", "Únor", "Březen", "Duben", "Květen", "Červen", "Červenec", "Srpen",
					"Září", "Říjen", "Listopad", "Prosinec"
				],
				monthNamesShort: ["Led", "Úno", "Bře", "Dub", "Kvě", "Čer", "Čvc", "Spr", "Zář", "Říj", "Lis",
					"Pro"
				],
				monthText: "Měsíc",
				secText: "Sekundy",
				timeFormat: "HH:ii",
				yearText: "Rok",
				nowText: "Teď",
				amText: "am",
				pmText: "pm",
				todayText: "Dnes",
				firstDay: 1,
				dateText: "Datum",
				timeText: "Čas",
				calendarText: "Kalendář",
				closeText: "Zavřít",
				fromText: "Začátek",
				toText: "Konec",
				wholeText: "Celý",
				fractionText: "Část",
				unitText: "Jednotka",
				labels: ["Roky", "Měsíce", "Dny", "Hodiny", "Minuty", "Sekundy", ""],
				labelsShort: ["Rok", "Měs", "Dny", "Hod", "Min", "Sec", ""],
				startText: "Start",
				stopText: "Stop",
				resetText: "Resetovat",
				lapText: "Etapa",
				hideText: "Schovat",
				backText: "Zpět",
				undoText: "Rozlepit",
				offText: "O",
				onText: "I",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.da = {
				setText: "Sæt",
				cancelText: "Annuller",
				clearText: "Ryd",
				selectedText: "{count} valgt",
				selectedPluralText: "{count} valgt",
				dateFormat: "dd/mm/yy",
				dayNames: ["Søndag", "Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag", "Lørdag"],
				dayNamesShort: ["Søn", "Man", "Tir", "Ons", "Tor", "Fre", "Lør"],
				dayNamesMin: ["S", "M", "T", "O", "T", "F", "L"],
				dayText: "Dag",
				hourText: "Timer",
				minuteText: "Minutter",
				monthNames: ["Januar", "Februar", "Marts", "April", "Maj", "Juni", "Juli", "August",
					"September", "Oktober", "November", "December"
				],
				monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov",
					"Dec"
				],
				monthText: "Måned",
				secText: "Sekunder",
				amText: "am",
				pmText: "pm",
				timeFormat: "HH.ii",
				yearText: "År",
				nowText: "Nu",
				todayText: "I dag",
				firstDay: 1,
				dateText: "Dato",
				timeText: "Tid",
				calendarText: "Kalender",
				closeText: "Luk",
				fromText: "Start",
				toText: "Slut",
				wholeText: "Hele",
				fractionText: "Dele",
				unitText: "Enhed",
				labels: ["År", "Måneder", "Dage", "Timer", "Minutter", "Sekunder", ""],
				labelsShort: ["År", "Mdr", "Dg", "Timer", "Min", "Sek", ""],
				startText: "Start",
				stopText: "Stop",
				resetText: "Nulstil",
				lapText: "Omgang",
				hideText: "Skjul",
				offText: "Fra",
				onText: "Til",
				backText: "Tilbage",
				undoText: "Fortryd"
			}
		}(),
		function() {
			t.i18n.de = {
				setText: "OK",
				cancelText: "Abbrechen",
				clearText: "Löschen",
				selectedText: "{count} ausgewählt",
				dateFormat: "dd.mm.yy",
				dayNames: ["Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag"],
				dayNamesShort: ["So", "Mo", "Di", "Mi", "Do", "Fr", "Sa"],
				dayNamesMin: ["S", "M", "D", "M", "D", "F", "S"],
				dayText: "Tag",
				delimiter: ".",
				hourText: "Stunde",
				minuteText: "Minuten",
				monthNames: ["Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September",
					"Oktober", "November", "Dezember"
				],
				monthNamesShort: ["Jan", "Feb", "Mär", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov",
					"Dez"
				],
				monthText: "Monat",
				secText: "Sekunden",
				timeFormat: "HH:ii",
				yearText: "Jahr",
				nowText: "Jetzt",
				pmText: "nachm.",
				amText: "vorm.",
				todayText: "Heute",
				firstDay: 1,
				dateText: "Datum",
				timeText: "Zeit",
				calendarText: "Kalender",
				closeText: "Schließen",
				fromText: "Von",
				toText: "Um",
				wholeText: "Ganze Zahl",
				fractionText: "Bruchzahl",
				unitText: "Maßeinheit",
				labels: ["Jahre", "Monate", "Tage", "Stunden", "Minuten", "Sekunden", ""],
				labelsShort: ["Jahr.", "Mon.", "Tag.", "Std.", "Min.", "Sek.", ""],
				startText: "Starten",
				stopText: "Stoppen",
				resetText: "Zurücksetzen",
				lapText: "Lap",
				hideText: "Ausblenden",
				backText: "Zurück",
				undoText: "Rückgängig machen",
				offText: "Aus",
				onText: "Ein",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n["en-GB"] = t.i18n["en-UK"] = {
				dateFormat: "dd/mm/yy",
				timeFormat: "HH:ii"
			}
		}(),
		function() {
			t.i18n.es = {
				setText: "Aceptar",
				cancelText: "Cancelar",
				clearText: "Borrar",
				selectedText: "{count} seleccionado",
				selectedPluralText: "{count} seleccionados",
				dateFormat: "dd/mm/yy",
				dayNames: ["Domingo", "Lunes", "Martes", "Mi&#xE9;rcoles", "Jueves", "Viernes", "S&#xE1;bado"],
				dayNamesShort: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "S&#xE1;"],
				dayNamesMin: ["D", "L", "M", "M", "J", "V", "S"],
				dayText: "D&#237;a",
				hourText: "Horas",
				minuteText: "Minutos",
				monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto",
					"Septiembre", "Octubre", "Noviembre", "Diciembre"
				],
				monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov",
					"Dic"
				],
				monthText: "Mes",
				secText: "Segundos",
				timeFormat: "HH:ii",
				yearText: "A&ntilde;o",
				nowText: "Ahora",
				pmText: "pm",
				amText: "am",
				todayText: "Hoy",
				firstDay: 1,
				dateText: "Fecha",
				timeText: "Tiempo",
				calendarText: "Calendario",
				closeText: "Cerrar",
				fromText: "Iniciar",
				toText: "Final",
				wholeText: "Entero",
				fractionText: "Fracción",
				unitText: "Unidad",
				labels: ["Años", "Meses", "Días", "Horas", "Minutos", "Segundos", ""],
				labelsShort: ["Año", "Mes", "Día", "Hora", "Min", "Seg", ""],
				startText: "Iniciar",
				stopText: "Deténgase",
				resetText: "Reinicializar",
				lapText: "Lap",
				hideText: "Esconder",
				backText: "Volver",
				undoText: "Deshacer",
				offText: "No",
				onText: "Sí",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			var e = {
				gDaysInMonth: [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31],
				jDaysInMonth: [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29]
			};
			e.jalaliToGregorian = function(t, a, s) {
				t = parseInt(t), a = parseInt(a), s = parseInt(s);
				var n, i = t - 979,
					r = a - 1,
					o = s - 1,
					l = 365 * i + 8 * parseInt(i / 33) + parseInt((i % 33 + 3) / 4);
				for (n = 0; r > n; ++n) l += e.jDaysInMonth[n];
				l += o;
				var c = l + 79,
					m = 1600 + 400 * parseInt(c / 146097);
				c %= 146097;
				var d = !0;
				for (c >= 36525 && (c--, m += 100 * parseInt(c / 36524), c %= 36524, c >= 365 ? c++ : d = !1),
					m += 4 * parseInt(c / 1461), c %= 1461, c >= 366 && (d = !1, c--, m += parseInt(c / 365),
						c %= 365), n = 0; c >= e.gDaysInMonth[n] + (1 == n && d); n++) c -= e.gDaysInMonth[n] +
					(1 == n && d);
				var u = n + 1,
					h = c + 1;
				return [m, u, h]
			}, e.checkDate = function(t, a, s) {
				return !(0 > t || t > 32767 || 1 > a || a > 12 || 1 > s || s > e.jDaysInMonth[a - 1] + (12 ==
					a && (t - 979) % 33 % 4 === 0))
			}, e.gregorianToJalali = function(t, a, s) {
				t = parseInt(t), a = parseInt(a), s = parseInt(s);
				var n, i = t - 1600,
					r = a - 1,
					o = s - 1,
					l = 365 * i + parseInt((i + 3) / 4) - parseInt((i + 99) / 100) + parseInt((i + 399) / 400);
				for (n = 0; r > n; ++n) l += e.gDaysInMonth[n];
				r > 1 && (i % 4 === 0 && i % 100 !== 0 || i % 400 === 0) && ++l, l += o;
				var c = l - 79,
					m = parseInt(c / 12053);
				c %= 12053;
				var d = 979 + 33 * m + 4 * parseInt(c / 1461);
				for (c %= 1461, c >= 366 && (d += parseInt((c - 1) / 365), c = (c - 1) % 365), n = 0; 11 > n &&
					c >= e.jDaysInMonth[n]; ++n) c -= e.jDaysInMonth[n];
				var u = n + 1,
					h = c + 1;
				return [d, u, h]
			}, t.i18n.fa = {
				setText: "تاييد",
				cancelText: "انصراف",
				clearText: "واضح ",
				selectedText: "{count} منتخب",
				dateFormat: "yy/mm/dd",
				dayNames: ["يکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه"],
				dayNamesShort: ["ی", "د", "س", "چ", "پ", "ج", "ش"],
				dayNamesMin: ["ی", "د", "س", "چ", "پ", "ج", "ش"],
				dayText: "روز",
				hourText: "ساعت",
				minuteText: "دقيقه",
				monthNames: ["فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان", "آذر",
					"دی", "بهمن", "اسفند"
				],
				monthNamesShort: ["فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان",
					"آذر", "دی", "بهمن", "اسفند"
				],
				monthText: "ماه",
				secText: "ثانيه",
				timeFormat: "HH:ii",
				yearText: "سال",
				nowText: "اکنون",
				amText: "ب",
				pmText: "ص",
				todayText: "امروز",
				getYear: function(t) {
					return e.gregorianToJalali(t.getFullYear(), t.getMonth() + 1, t.getDate())[0]
				},
				getMonth: function(t) {
					return --e.gregorianToJalali(t.getFullYear(), t.getMonth() + 1, t.getDate())[1]
				},
				getDay: function(t) {
					return e.gregorianToJalali(t.getFullYear(), t.getMonth() + 1, t.getDate())[2]
				},
				getDate: function(t, a, s, n, i, r, o) {
					0 > a && (t += Math.floor(a / 12), a = 12 + a % 12), a > 11 && (t += Math.floor(a / 12),
						a %= 12);
					var l = e.jalaliToGregorian(t, +a + 1, s);
					return new Date(l[0], l[1] - 1, l[2], n || 0, i || 0, r || 0, o || 0)
				},
				getMaxDayOfMonth: function(t, a) {
					for (var s = 31; e.checkDate(t, a + 1, s) === !1;) s--;
					return s
				},
				firstDay: 6,
				rtl: !0,
				dateText: "تاریخ ",
				timeText: "زمان ",
				calendarText: "تقویم",
				closeText: "نزدیک",
				fromText: "شروع ",
				toText: "پایان",
				wholeText: "تمام",
				fractionText: "کسر",
				unitText: "واحد",
				labels: ["سال", "ماه", "روز", "ساعت", "دقیقه", "ثانیه", ""],
				labelsShort: ["سال", "ماه", "روز", "ساعت", "دقیقه", "ثانیه", ""],
				startText: "شروع",
				stopText: "پايان",
				resetText: "تنظیم مجدد",
				lapText: "Lap",
				hideText: "پنهان کردن",
				backText: "پشت",
				undoText: "واچیدن"
			}
		}(),
		function() {
			t.i18n.fr = {
				setText: "Terminer",
				cancelText: "Annuler",
				clearText: "Effacer",
				selectedText: "{count} sélectionné",
				selectedPluralText: "{count} sélectionnés",
				dateFormat: "dd/mm/yy",
				dayNames: ["&#68;imanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"],
				dayNamesShort: ["&#68;im.", "Lun.", "Mar.", "Mer.", "Jeu.", "Ven.", "Sam."],
				dayNamesMin: ["&#68;", "L", "M", "M", "J", "V", "S"],
				dayText: "Jour",
				monthText: "Mois",
				monthNames: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août",
					"Septembre", "Octobre", "Novembre", "Décembre"
				],
				monthNamesShort: ["Janv.", "Févr.", "Mars", "Avril", "Mai", "Juin", "Juil.", "Août", "Sept.",
					"Oct.", "Nov.", "Déc."
				],
				hourText: "Heures",
				minuteText: "Minutes",
				secText: "Secondes",
				timeFormat: "HH:ii",
				yearText: "Année",
				nowText: "Maintenant",
				pmText: "après-midi",
				amText: "avant-midi",
				todayText: "Aujourd'hui",
				firstDay: 1,
				dateText: "Date",
				timeText: "Heure",
				calendarText: "Calendrier",
				closeText: "Fermer",
				fromText: "Démarrer",
				toText: "Fin",
				wholeText: "Entier",
				fractionText: "Fraction",
				unitText: "Unité",
				labels: ["Ans", "Mois", "Jours", "Heures", "Minutes", "Secondes", ""],
				labelsShort: ["Ans", "Mois", "Jours", "Hrs", "Min", "Sec", ""],
				startText: "Démarrer",
				stopText: "Arrêter",
				resetText: "Réinitialiser",
				lapText: "Lap",
				hideText: "Cachez",
				backText: "Arrière",
				undoText: "Défaire",
				offText: "Non",
				onText: "Oui",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.he = {
				rtl: !0,
				setText: "שמירה",
				cancelText: "ביטול",
				clearText: "נקה",
				selectedText: "{count} נבחר",
				selectedPluralText: "{count} נבחרו",
				dateFormat: "dd/mm/yy",
				dayNames: ["ראשון", "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת"],
				dayNamesShort: ["א'", "ב'", "ג'", "ד'", "ה'", "ו'", "ש'"],
				dayNamesMin: ["א", "ב", "ג", "ד", "ה", "ו", "ש"],
				dayText: "יום",
				hourText: "שעות",
				minuteText: "דקות",
				monthNames: ["ינואר", "פברואר", "מרץ", "אפריל", "מאי", "יוני", "יולי", "אוגוסט", "ספטמבר",
					"אוקטובר", "נובמבר", "דצמבר"
				],
				monthNamesShort: ["ינו", "פבר", "מרץ", "אפר", "מאי", "יונ", "יול", "אוג", "ספט", "אוק", "נוב",
					"דצמ"
				],
				monthText: "חודש",
				secText: "שניות",
				amText: "am",
				pmText: "pm",
				timeFormat: "HH:ii",
				yearText: "שנה",
				nowText: "עכשיו",
				firstDay: 0,
				dateText: "תאריך",
				timeText: "זמן",
				calendarText: "תאריכון",
				closeText: "סגירה",
				todayText: "היום",
				eventText: "מִקרֶה",
				eventsText: "מִקרֶה",
				fromText: "התחלה",
				toText: "סיום",
				wholeText: "כֹּל",
				fractionText: "שבריר",
				unitText: "יחידה",
				labels: ["שנים", "חודשים", "ימים", "שעות", "דקות", "שניים", ""],
				labelsShort: ["שנים", "חודשים", "ימים", "שעות", "דקות", "שניים", ""],
				startText: "התחל",
				stopText: "עצור",
				resetText: "אתחול",
				lapText: "הקפה",
				hideText: "הסתר",
				offText: "כיבוי",
				onText: "הפעלה",
				backText: "חזור",
				undoText: "ביטול פעולה"
			}
		}(),
		function() {
			t.i18n.hu = {
				setText: "OK",
				cancelText: "Mégse",
				clearText: "Törlés",
				selectedText: "{count} kiválasztva",
				dateFormat: "yy.mm.dd.",
				dayNames: ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"],
				dayNamesShort: ["Va", "Hé", "Ke", "Sze", "Csü", "Pé", "Szo"],
				dayNamesMin: ["V", "H", "K", "Sz", "Cs", "P", "Sz"],
				dayText: "Nap",
				delimiter: ".",
				hourText: "Óra",
				minuteText: "Perc",
				monthNames: ["Január", "Február", "Március", "Április", "Május", "Június", "Július",
					"Augusztus", "Szeptember", "Október", "November", "December"
				],
				monthNamesShort: ["Jan", "Feb", "Már", "Ápr", "Máj", "Jún", "Júl", "Aug", "Szep", "Okt", "Nov",
					"Dec"
				],
				monthText: "Hónap",
				secText: "Másodperc",
				timeFormat: "H:ii",
				yearText: "Év",
				nowText: "Most",
				pmText: "de",
				amText: "du",
				firstDay: 1,
				dateText: "Dátum",
				timeText: "Idő",
				calendarText: "Naptár",
				todayText: "Ma",
				prevMonthText: "Előző hónap",
				nextMonthText: "Következő hónap",
				prevYearText: "Előző év",
				nextYearText: "Következő év",
				closeText: "Bezár",
				eventText: "esemény",
				eventsText: "esemény",
				fromText: "Eleje",
				toText: "Vége",
				wholeText: "Egész",
				fractionText: "Tört",
				unitText: "Egység",
				labels: ["Év", "Hónap", "Nap", "Óra", "Perc", "Másodperc", ""],
				labelsShort: ["Év", "Hó.", "Nap", "Óra", "Perc", "Mp.", ""],
				startText: "Indít",
				stopText: "Megállít",
				resetText: "Visszaállít",
				lapText: "Lap",
				hideText: "Elrejt",
				backText: "Vissza",
				undoText: "Visszavon",
				offText: "Ki",
				onText: "Be",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.it = {
				setText: "OK",
				cancelText: "Annulla",
				clearText: "Chiarire",
				selectedText: "{count} selezionato",
				selectedPluralText: "{count} selezionati",
				dateFormat: "dd/mm/yy",
				dayNames: ["Domenica", "Lunedì", "Mertedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato"],
				dayNamesShort: ["Do", "Lu", "Ma", "Me", "Gi", "Ve", "Sa"],
				dayNamesMin: ["D", "L", "M", "M", "G", "V", "S"],
				dayText: "Giorno",
				hourText: "Ore",
				minuteText: "Minuti",
				monthNames: ["Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto",
					"Settembre", "Ottobre", "Novembre", "Dicembre"
				],
				monthNamesShort: ["Gen", "Feb", "Mar", "Apr", "Mag", "Giu", "Lug", "Ago", "Set", "Ott", "Nov",
					"Dic"
				],
				monthText: "Mese",
				secText: "Secondi",
				timeFormat: "HH:ii",
				yearText: "Anno",
				nowText: "Ora",
				pmText: "pm",
				amText: "am",
				todayText: "Oggi",
				firstDay: 1,
				dateText: "Data",
				timeText: "Volta",
				calendarText: "Calendario",
				closeText: "Chiudere",
				fromText: "Inizio",
				toText: "Fine",
				wholeText: "Intero",
				fractionText: "Frazione",
				unitText: "Unità",
				labels: ["Anni", "Mesi", "Giorni", "Ore", "Minuti", "Secondi", ""],
				labelsShort: ["Anni", "Mesi", "Gio", "Ore", "Min", "Sec", ""],
				startText: "Inizio",
				stopText: "Arresto",
				resetText: "Ripristina",
				lapText: "Lap",
				hideText: "Nascondi",
				backText: "Indietro",
				undoText: "Annulla",
				offText: "Via",
				onText: "Su",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.ja = {
				setText: "セット",
				cancelText: "キャンセル",
				clearText: "クリア",
				selectedText: "{count} 選択",
				dateFormat: "yy年mm月dd日",
				dayNames: ["日", "月", "火", "水", "木", "金", "土"],
				dayNamesShort: ["日", "月", "火", "水", "木", "金", "土"],
				dayNamesMin: ["日", "月", "火", "水", "木", "金", "土"],
				dayText: "日",
				hourText: "時",
				minuteText: "分",
				monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
				monthNamesShort: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
				monthText: "月",
				secText: "秒",
				timeFormat: "HH:ii",
				yearText: "年",
				nowText: "今",
				pmText: "午後",
				amText: "午前",
				yearSuffix: "年",
				monthSuffix: "月",
				daySuffix: "日",
				todayText: "今日",
				dateText: "日付",
				timeText: "時間",
				calendarText: "カレンダー",
				closeText: "クローズ",
				fromText: "開始",
				toText: "終わり",
				wholeText: "全数",
				fractionText: "分数",
				unitText: "単位",
				labels: ["年間", "月間", "日間", "時間", "分", "秒", ""],
				labelsShort: ["年間", "月間", "日間", "時間", "分", "秒", ""],
				startText: "開始",
				stopText: "停止",
				resetText: "リセット",
				lapText: "ラップ",
				hideText: "隠す",
				backText: "バック",
				undoText: "アンドゥ"
			}
		}(),
		function() {
			t.i18n.lt = {
				setText: "OK",
				cancelText: "Atšaukti",
				clearText: "Išvalyti",
				selectedText: "Pasirinktas {count}",
				selectedPluralText: "Pasirinkti {count}",
				dateFormat: "yy-mm-dd",
				dayNames: ["Sekmadienis", "Pirmadienis", "Antradienis", "Trečiadienis", "Ketvirtadienis",
					"Penktadienis", "Šeštadienis"
				],
				dayNamesShort: ["S", "Pr", "A", "T", "K", "Pn", "Š"],
				dayNamesMin: ["S", "Pr", "A", "T", "K", "Pn", "Š"],
				dayText: "Diena",
				hourText: "Valanda",
				minuteText: "Minutes",
				monthNames: ["Sausis", "Vasaris", "Kovas", "Balandis", "Gegužė", "Birželis", "Liepa",
					"Rugpjūtis", "Rugsėjis", "Spalis", "Lapkritis", "Gruodis"
				],
				monthNamesShort: ["Sau", "Vas", "Kov", "Bal", "Geg", "Bir", "Lie", "Rugp", "Rugs", "Spa", "Lap",
					"Gruo"
				],
				monthText: "Mėnuo",
				secText: "Sekundes",
				amText: "am",
				pmText: "pm",
				timeFormat: "HH:ii",
				yearText: "Metai",
				nowText: "Dabar",
				todayText: "Šiandien",
				firstDay: 1,
				dateText: "Data",
				timeText: "Laikas",
				calendarText: "Kalendorius",
				closeText: "Uždaryti",
				fromText: "Nuo",
				toText: "Iki",
				wholeText: "Visas",
				fractionText: "Frakcija",
				unitText: "Vienetas",
				labels: ["Metai", "Mėnesiai", "Dienos", "Valandos", "Minutes", "Sekundes", ""],
				labelsShort: ["m", "mėn.", "d", "h", "min", "s", ""],
				startText: "Pradėti",
				stopText: "Sustabdyti",
				resetText: "Išnaujo",
				lapText: "Ratas",
				hideText: "Slėpti",
				backText: "Atgal",
				undoText: "Atšaukti veiksmą",
				offText: "Išj.",
				onText: "Įj.",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.nl = {
				setText: "Instellen",
				cancelText: "Annuleren",
				clearText: "Duidelijk",
				selectedText: "{count} gekozen",
				dateFormat: "dd-mm-yy",
				dayNames: ["zondag", "maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag"],
				dayNamesShort: ["zo", "ma", "di", "wo", "do", "vr", "za"],
				dayNamesMin: ["z", "m", "d", "w", "d", "v", "z"],
				dayText: "Dag",
				hourText: "Uur",
				minuteText: "Minuten",
				monthNames: ["januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus",
					"september", "oktober", "november", "december"
				],
				monthNamesShort: ["jan", "feb", "mrt", "apr", "mei", "jun", "jul", "aug", "sep", "okt", "nov",
					"dec"
				],
				monthText: "Maand",
				secText: "Seconden",
				timeFormat: "HH:ii",
				yearText: "Jaar",
				nowText: "Nu",
				pmText: "pm",
				amText: "am",
				todayText: "Vandaag",
				firstDay: 1,
				dateText: "Datum",
				timeText: "Tijd",
				calendarText: "Kalender",
				closeText: "Sluiten",
				fromText: "Start",
				toText: "Einde",
				wholeText: "geheel",
				fractionText: "fractie",
				unitText: "eenheid",
				labels: ["Jaren", "Maanden", "Dagen", "Uren", "Minuten", "Seconden", ""],
				labelsShort: ["j", "m", "d", "u", "min", "sec", ""],
				startText: "Start",
				stopText: "Stop",
				resetText: "Reset",
				lapText: "Ronde",
				hideText: "Verbergen",
				backText: "Terug",
				undoText: "Onged. maken",
				offText: "Uit",
				onText: "Aan",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.no = {
				setText: "OK",
				cancelText: "Avbryt",
				clearText: "Tømme",
				selectedText: "{count} valgt",
				dateFormat: "dd.mm.yy",
				dayNames: ["Søndag", "Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag", "Lørdag"],
				dayNamesShort: ["Sø", "Ma", "Ti", "On", "To", "Fr", "Lø"],
				dayNamesMin: ["S", "M", "T", "O", "T", "F", "L"],
				dayText: "Dag",
				delimiter: ".",
				hourText: "Time",
				minuteText: "Minutt",
				monthNames: ["Januar", "Februar", "Mars", "April", "Mai", "Juni", "Juli", "August", "September",
					"Oktober", "November", "Desember"
				],
				monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov",
					"Des"
				],
				monthText: "Måned",
				secText: "Sekund",
				timeFormat: "HH:ii",
				yearText: "År",
				nowText: "Nå",
				pmText: "pm",
				amText: "am",
				todayText: "I dag",
				firstDay: 1,
				dateText: "Dato",
				timeText: "Tid",
				calendarText: "Kalender",
				closeText: "Lukk",
				fromText: "Start",
				toText: "End",
				wholeText: "Hele",
				fractionText: "Fraksjon",
				unitText: "Enhet",
				labels: ["År", "Måneder", "Dager", "Timer", "Minutter", "Sekunder", ""],
				labelsShort: ["År", "Mån", "Dag", "Time", "Min", "Sek", ""],
				startText: "Start",
				stopText: "Stopp",
				resetText: "Tilbakestille",
				lapText: "Runde",
				hideText: "Skjul",
				backText: "Tilbake",
				undoText: "Angre",
				offText: "Av",
				onText: "På",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.pl = {
				setText: "Zestaw",
				cancelText: "Anuluj",
				clearText: "Oczyścić",
				selectedText: "Wybór: {count}",
				dateFormat: "yy-mm-dd",
				dayNames: ["Niedziela", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota"],
				dayNamesShort: ["Niedz.", "Pon.", "Wt.", "Śr.", "Czw.", "Pt.", "Sob."],
				dayNamesMin: ["N", "P", "W", "Ś", "C", "P", "S"],
				dayText: "Dzień",
				hourText: "Godziny",
				minuteText: "Minuty",
				monthNames: ["Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień",
					"Wrzesień", "Październik", "Listopad", "Grudzień"
				],
				monthNamesShort: ["Sty", "Lut", "Mar", "Kwi", "Maj", "Cze", "Lip", "Sie", "Wrz", "Paź", "Lis",
					"Gru"
				],
				monthText: "Miesiąc",
				secText: "Sekundy",
				timeFormat: "HH:ii",
				yearText: "Rok",
				nowText: "Teraz",
				amText: "rano",
				pmText: "po południu",
				todayText: "Dzisiaj",
				firstDay: 1,
				dateText: "Data",
				timeText: "Czas",
				calendarText: "Kalendarz",
				closeText: "Zakończenie",
				fromText: "Rozpoczęcie",
				toText: "Koniec",
				wholeText: "Cały",
				fractionText: "Ułamek",
				unitText: "Jednostka",
				labels: ["Lata", "Miesiąc", "Dni", "Godziny", "Minuty", "Sekundy", ""],
				labelsShort: ["R", "M", "Dz", "Godz", "Min", "Sek", ""],
				startText: "Rozpoczęcie",
				stopText: "Zatrzymać",
				resetText: "Zresetować",
				lapText: "Zakładka",
				hideText: "Ukryć",
				backText: "Z powrotem",
				undoText: "Cofnij",
				offText: "Wył",
				onText: "Wł",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n["pt-BR"] = {
				setText: "Selecionar",
				cancelText: "Cancelar",
				clearText: "Claro",
				selectedText: "{count} selecionado",
				selectedPluralText: "{count} selecionados",
				dateFormat: "dd/mm/yy",
				dayNames: ["Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira",
					"Sexta-feira", "Sábado"
				],
				dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"],
				dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S"],
				dayText: "Dia",
				hourText: "Hora",
				minuteText: "Minutos",
				monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto",
					"Setembro", "Outubro", "Novembro", "Dezembro"
				],
				monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov",
					"Dez"
				],
				monthText: "Mês",
				secText: "Segundo",
				timeFormat: "HH:ii",
				yearText: "Ano",
				nowText: "Agora",
				pmText: "da tarde",
				amText: "da manhã",
				todayText: "Hoje",
				dateText: "Data",
				timeText: "Tempo",
				calendarText: "Calendário",
				closeText: "Fechar",
				fromText: "In&iacute;cio",
				toText: "Fim",
				wholeText: "Inteiro",
				fractionText: "Fração",
				unitText: "Unidade",
				labels: ["Anos", "Meses", "Dias", "Horas", "Minutos", "Segundos", ""],
				labelsShort: ["Ano", "M&ecirc;s", "Dia", "Hora", "Min", "Seg", ""],
				startText: "Começar",
				stopText: "Pare",
				resetText: "Reinicializar",
				lapText: "Lap",
				hideText: "Esconder",
				backText: "De volta",
				undoText: "Desfazer",
				offText: "Desl",
				onText: "Lig",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n["pt-PT"] = {
				setText: "Seleccionar",
				cancelText: "Cancelar",
				clearText: "Claro",
				selectedText: "{count} selecionado",
				selectedPluralText: "{count} selecionados",
				dateFormat: "dd-mm-yy",
				dayNames: ["Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira",
					"Sexta-feira", "S&aacute;bado"
				],
				dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "S&aacute;b"],
				dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S"],
				dayText: "Dia",
				hourText: "Horas",
				minuteText: "Minutos",
				monthNames: ["Janeiro", "Fevereiro", "Mar&ccedil;o", "Abril", "Maio", "Junho", "Julho",
					"Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
				],
				monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov",
					"Dez"
				],
				monthText: "M&ecirc;s",
				secText: "Segundo",
				timeFormat: "HH:ii",
				yearText: "Ano",
				nowText: "Actualizar",
				pmText: "da tarde",
				amText: "da manhã",
				todayText: "Hoy",
				firstDay: 1,
				dateText: "Data",
				timeText: "Tempo",
				calendarText: "Calend&aacute;rio",
				closeText: "Fechar",
				fromText: "In&iacute;cio",
				toText: "Fim",
				wholeText: "Inteiro",
				fractionText: "Frac&ccedil;&atilde;o",
				unitText: "Unidade",
				labels: ["Anos", "Meses", "Dias", "Horas", "Minutos", "Segundos", ""],
				labelsShort: ["Ano", "M&ecirc;s", "Dia", "Hora", "Min", "Seg", ""],
				startText: "Come&ccedil;ar",
				stopText: "Parar",
				resetText: "Reinicializar",
				lapText: "Lap",
				hideText: "Esconder",
				backText: "De volta",
				undoText: "Anular",
				offText: "Desl",
				onText: "Lig",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.ro = {
				setText: "Setare",
				cancelText: "Anulare",
				clearText: "Ştergere",
				selectedText: "{count} selectat",
				selectedPluralText: "{count} selectate",
				dateFormat: "dd.mm.yy",
				dayNames: ["Duminică", "Luni", "Marți", "Miercuri", "Joi", "Vineri", "Sâmbătă"],
				dayNamesShort: ["Du", "Lu", "Ma", "Mi", "Jo", "Vi", "Sâ"],
				dayNamesMin: ["D", "L", "M", "M", "J", "V", "S"],
				dayText: " Ziua",
				delimiter: ".",
				hourText: " Ore ",
				minuteText: "Minute",
				monthNames: ["Ianuarie", "Februarie", "Martie", "Aprilie", "Mai", "Iunie", "Iulie", "August",
					"Septembrie", "Octombrie", "Noiembrie", "Decembrie"
				],
				monthNamesShort: ["Ian.", "Feb.", "Mar.", "Apr.", "Mai", "Iun.", "Iul.", "Aug.", "Sept.",
					"Oct.", "Nov.", "Dec."
				],
				monthText: "Luna",
				secText: "Secunde",
				timeFormat: "HH:ii",
				yearText: "Anul",
				nowText: "Acum",
				amText: "am",
				pmText: "pm",
				todayText: "Astăzi",
				firstDay: 1,
				dateText: "Data",
				timeText: "Ora",
				calendarText: "Calendar",
				closeText: "Închidere",
				fromText: "Start",
				toText: "Final",
				wholeText: "Complet",
				fractionText: "Parţial",
				unitText: "Unitate",
				labels: ["Ani", "Luni", "Zile", "Ore", "Minute", "Secunde", ""],
				labelsShort: ["Ani", "Luni", "Zile", "Ore", "Min.", "Sec.", ""],
				startText: "Start",
				stopText: "Stop",
				resetText: "Resetare",
				lapText: "Tură",
				hideText: "Ascundere",
				backText: "Înapoi",
				undoText: "Anulaţi",
				offText: "Nu",
				onText: "Da",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n["ru-UA"] = {
				setText: "Установить",
				cancelText: "Отменить",
				clearText: "Очиститьr",
				selectedText: "{count} Вібрать",
				dateFormat: "dd.mm.yy",
				dayNames: ["воскресенье", "понедельник", "вторник", "среда", "четверг", "пятница", "суббота"],
				dayNamesShort: ["вс", "пн", "вт", "ср", "чт", "пт", "сб"],
				dayNamesMin: ["в", "п", "в", "с", "ч", "п", "с"],
				dayText: "День",
				delimiter: ".",
				hourText: "Часы",
				minuteText: "Минуты",
				monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь",
					"Октябрь", "Ноябрь", "Декабрь"
				],
				monthNamesShort: ["Янв.", "Февр.", "Март", "Апр.", "Май", "Июнь", "Июль", "Авг.", "Сент.",
					"Окт.", "Нояб.", "Дек."
				],
				monthText: "Месяцы",
				secText: "Сикунды",
				timeFormat: "HH:ii",
				yearText: "Год",
				nowText: "Сейчас",
				amText: "До полудня",
				pmText: "После полудня",
				todayText: "Cегодня",
				firstDay: 1,
				dateText: "Дата",
				timeText: "Время",
				calendarText: "Календарь",
				closeText: "Закрыть",
				fromText: "Начало",
				toText: "Конец",
				wholeText: "Весь",
				fractionText: "Часть",
				unitText: "Единица",
				labels: ["Годы", " Месяцы ", " Дни ", " Часы ", " Минуты ", " Секунды", ""],
				labelsShort: ["Год", "Мес.", "Дн.", "Ч.", "Мин.", "Сек.", ""],
				startText: "Старт",
				stopText: "Стоп",
				resetText: " Сброс ",
				lapText: " Этап ",
				hideText: " Скрыть ",
				backText: "назад",
				undoText: "аннулировать",
				offText: "O",
				onText: "I",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n["ru-RU"] = t.i18n.ru = {
				setText: "Установить",
				cancelText: "Отмена",
				clearText: "Очистить",
				selectedText: "{count} Выбрать",
				dateFormat: "dd.mm.yy",
				dayNames: ["воскресенье", "понедельник", "вторник", "среда", "четверг", "пятница", "суббота"],
				dayNamesShort: ["вс", "пн", "вт", "ср", "чт", "пт", "сб"],
				dayNamesMin: ["в", "п", "в", "с", "ч", "п", "с"],
				dayText: "День",
				delimiter: ".",
				hourText: "Час",
				minuteText: "Минут",
				monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь",
					"Октябрь", "Ноябрь", "Декабрь"
				],
				monthNamesShort: ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя",
					"Дек"
				],
				monthText: "Месяц",
				secText: "Секунд",
				timeFormat: "HH:ii",
				yearText: "Год",
				nowText: "Сейчас",
				amText: "До полудня",
				pmText: "После полудня",
				todayText: "Cегодня",
				firstDay: 1,
				dateText: "Дата",
				timeText: "Время",
				calendarText: "Календарь",
				closeText: "Закрыть",
				fromText: "Начало",
				toText: "Конец",
				wholeText: "Целое",
				fractionText: "Дробное",
				unitText: "Единица",
				labels: ["Лет", "Месяцев", "Дней", "Часов", "Минут", "Секунд", ""],
				labelsShort: ["Лет", "Мес", "Дн", "Час", "Мин", "Сек", ""],
				startText: "Старт",
				stopText: "Стоп",
				resetText: "Сбросить",
				lapText: "Круг",
				hideText: "Скрыть",
				backText: "назад",
				undoText: "аннулировать",
				offText: "O",
				onText: "I",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.sk = {
				setText: "Zadaj",
				cancelText: "Zrušiť",
				clearText: "Vymazať",
				selectedText: "Označený: {count}",
				dateFormat: "d.m.yy",
				dayNames: ["Nedeľa", "Pondelok", "Utorok", "Streda", "Štvrtok", "Piatok", "Sobota"],
				dayNamesShort: ["Ne", "Po", "Ut", "St", "Št", "Pi", "So"],
				dayNamesMin: ["N", "P", "U", "S", "Š", "P", "S"],
				dayText: "Ďeň",
				hourText: "Hodiny",
				minuteText: "Minúty",
				monthNames: ["Január", "Február", "Marec", "Apríl", "Máj", "Jún", "Júl", "August", "September",
					"Október", "November", "December"
				],
				monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Máj", "Jún", "Júl", "Aug", "Sep", "Okt", "Nov",
					"Dec"
				],
				monthText: "Mesiac",
				secText: "Sekundy",
				timeFormat: "H:ii",
				yearText: "Rok",
				nowText: "Teraz",
				amText: "am",
				pmText: "pm",
				todayText: "Dnes",
				firstDay: 1,
				dateText: "Datum",
				timeText: "Čas",
				calendarText: "Kalendár",
				closeText: "Zavrieť",
				fromText: "Začiatok",
				toText: "Koniec",
				wholeText: "Celý",
				fractionText: "Časť",
				unitText: "Jednotka",
				labels: ["Roky", "Mesiace", "Dni", "Hodiny", "Minúty", "Sekundy", ""],
				labelsShort: ["Rok", "Mes", "Dni", "Hod", "Min", "Sec", ""],
				startText: "Start",
				stopText: "Stop",
				resetText: "Resetovať",
				lapText: "Etapa",
				hideText: "Schovať",
				backText: "Späť",
				undoText: "Späť",
				offText: "O",
				onText: "I",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function() {
			t.i18n.sv = {
				setText: "OK",
				cancelText: "Avbryt",
				clearText: "Klara",
				selectedText: "{count} vald",
				dateFormat: "yy-mm-dd",
				dayNames: ["Söndag", "Måndag", "Tisdag", "Onsdag", "Torsdag", "Fredag", "Lördag"],
				dayNamesShort: ["Sö", "Må", "Ti", "On", "To", "Fr", "Lö"],
				dayNamesMin: ["S", "M", "T", "O", "T", "F", "L"],
				dayText: "Dag",
				hourText: "Timme",
				minuteText: "Minut",
				monthNames: ["Januari", "Februari", "Mars", "April", "Maj", "Juni", "Juli", "Augusti",
					"September", "Oktober", "November", "December"
				],
				monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov",
					"Dec"
				],
				monthText: "Månad",
				secText: "Sekund",
				timeFormat: "HH:ii",
				yearText: "År",
				nowText: "Nu",
				pmText: "pm",
				amText: "am",
				todayText: "I dag",
				firstDay: 1,
				dateText: "Datum",
				timeText: "Tid",
				calendarText: "Kalender",
				closeText: "Stäng",
				fromText: "Start",
				toText: "Slut",
				wholeText: "Hela",
				fractionText: "Bråk",
				unitText: "Enhet",
				labels: ["År", "Månader", "Dagar", "Timmar", "Minuter", "Sekunder", ""],
				labelsShort: ["År", "Mån", "Dag", "Tim", "Min", "Sek", ""],
				startText: "Start",
				stopText: "Stopp",
				resetText: "Återställ",
				lapText: "Varv",
				hideText: "Dölj",
				backText: "Tillbaka",
				undoText: "Ångra",
				offText: "Av",
				onText: "På"
			}
		}(),
		function() {
			t.i18n.tr = {
				setText: "Seç",
				cancelText: "İptal",
				clearText: "Temizleyin",
				selectedText: "{count} seçilmiş",
				dateFormat: "dd.mm.yy",
				dayNames: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
				dayNamesShort: ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"],
				dayNamesMin: ["P", "P", "S", "Ç", "P", "C", "C"],
				dayText: "Gün",
				delimiter: ".",
				hourText: "Saat",
				minuteText: "Dakika",
				monthNames: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül",
					"Ekim", "Kasım", "Aralık"
				],
				monthNamesShort: ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas",
					"Ara"
				],
				monthText: "Ay",
				secText: "Saniye",
				timeFormat: "HH:ii",
				yearText: "Yıl",
				nowText: "Şimdi",
				pmText: "akşam",
				amText: "sabah",
				todayText: "Bugün",
				firstDay: 1,
				dateText: "Tarih",
				timeText: "Zaman",
				calendarText: "Takvim",
				closeText: "Kapatmak",
				fromText: "Başla",
				toText: "Son",
				wholeText: "Tam",
				fractionText: "Kesir",
				unitText: "Birim",
				labels: ["Yıl", "Ay", "Gün", "Saat", "Dakika", "Saniye", ""],
				labelsShort: ["Yıl", "Ay", "Gün", "Sa", "Dak", "Sn", ""],
				startText: "Başla",
				stopText: "Durdur",
				resetText: "Sıfırla",
				lapText: "Tur",
				hideText: "Gizle",
				backText: "Geri",
				undoText: "Geri Al",
				offText: "O",
				onText: "I",
				decimalSeparator: ",",
				thousandsSeparator: "."
			}
		}(),
		function() {
			t.i18n.zh = {
				setText: "确定",
				cancelText: "取消",
				clearText: "明确",
				selectedText: "{count} 选",
				dateFormat: "yy/mm/dd",
				dayNames: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
				dayNamesShort: ["日", "一", "二", "三", "四", "五", "六"],
				dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
				dayText: "日",
				hourText: "时",
				minuteText: "分",
				monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
				monthNamesShort: ["一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二"],
				monthText: "月",
				secText: "秒",
				timeFormat: "HH:ii",
				yearText: "年",
				nowText: "当前",
				pmText: "下午",
				amText: "上午",
				todayText: "今天",
				dateText: "日",
				timeText: "时间",
				calendarText: "日历",
				closeText: "关闭",
				fromText: "开始时间",
				toText: "结束时间",
				wholeText: "合计",
				fractionText: "分数",
				unitText: "单位",
				labels: ["年", "月", "日", "小时", "分钟", "秒", ""],
				labelsShort: ["年", "月", "日", "点", "分", "秒", ""],
				startText: "开始",
				stopText: "停止",
				resetText: "重置",
				lapText: "圈",
				hideText: "隐藏",
				backText: "背部",
				undoText: "复原",
				offText: "关闭",
				onText: "开启",
				decimalSeparator: ",",
				thousandsSeparator: " "
			}
		}(),
		function(e, a, s) {
			var n, i, r = t,
				o = r.$,
				l = r.platform,
				c = r.util,
				m = c.constrain,
				d = c.isString,
				u = c.getCoord,
				h = /(iphone|ipod)/i.test(navigator.userAgent) && l.majorVersion >= 7,
				f = "ios" == l.name && 8 == l.majorVersion,
				p = "webkitAnimationEnd.mbsc animationend.mbsc",
				b = function() {},
				v = function(e) {
					e.preventDefault()
				};
			r.classes.Frame = function(t, l, f) {
				function g(e) {
					j && j.removeClass("mbsc-fr-btn-a"), j = o(this), j.hasClass("mbsc-fr-btn-d") || j.hasClass(
						"mbsc-fr-btn-nhl") || j.addClass("mbsc-fr-btn-a"), "mousedown" === e.type ? o(a).on(
						"mouseup", x) : "pointerdown" === e.type && o(a).on("pointerup", x)
				}

				function x(e) {
					j && (j.removeClass("mbsc-fr-btn-a"), j = null), "mouseup" === e.type ? o(a).off("mouseup",
						x) : "pointerup" === e.type && o(a).off("pointerup", x)
				}

				function T(e) {
					13 == e.keyCode ? ct.select() : 27 == e.keyCode && ct.cancel()
				}

				function y(e) {
					e || Q.focus(), ct.ariaMessage(st.ariaMessage)
				}

				function w(e) {
					var t = n,
						a = st.focusOnClose;
					ct._markupRemove(), I.remove(), R && (L.removeClass(q), Z && (N.css({
						top: "",
						left: ""
					}), E.scrollLeft(nt), E.scrollTop(rt))), e || (t || (t = mt), setTimeout(function() {
						r.activeInstance || (a === s || a === !0 ? (i = !0, t[0].focus()) : a && o(
							a)[0].focus())
					}, 200)), n = null, ct._isVisible = !1, B = !1, $("onHide")
				}

				function C(e) {
					clearTimeout(ut[e.type]), ut[e.type] = setTimeout(function() {
						var t, a = "scroll" == e.type;
						(!a || it) && (ct.position(!a), "orientationchange" == e.type && (et.style
							.display = "none", t = et.offsetHeight, et.style.display = ""))
					}, 200)
				}

				function M(e) {
					e.target.nodeType && !et.contains(e.target) && et.focus()
				}

				function S() {
					o(a.activeElement).is("input,textarea") && a.activeElement.blur()
				}

				function D(e, t) {
					e && e(), ct.show() !== !1 && (n = t, setTimeout(function() {
						i = !1
					}, 300))
				}

				function k() {
					ct._fillValue(), $("onSet", {
						valueText: ct._value
					})
				}

				function _() {
					$("onCancel", {
						valueText: ct._value
					})
				}

				function A() {
					ct.setVal(null, !0)
				}
				var V, N, F, L, I, H, O, P, E, Y, z, j, W, $, J, R, B, q, U, K, G, X, Z, Q, et, tt, at, st, nt,
					it, rt, ot, lt, ct = this,
					mt = o(t),
					dt = [],
					ut = {};
				r.classes.Base.call(this, t, l, !0), ct.position = function(e) {
						var t, n, i, l, c, d, u, h, f, p, b, v, g, x, T, y, w = {},
							C = 0,
							M = 0,
							S = 0,
							D = 0;
						!at && B && (v = U.offsetHeight, g = U.offsetWidth, ot === g && lt === v && e || ((ct
							._isFullScreen || /top|bottom/.test(st.display)) && P.width(g), $(
							"onPosition", {
								target: U,
								windowWidth: g,
								windowHeight: v
							}) !== !1 && R && (o(".mbsc-comp", I).each(function() {
								var e = r.instances[this.id];
								e && e !== ct && e.position && e.position()
							}), !ct._isFullScreen && /center|bubble/.test(st.display) && (o(
								".mbsc-w-p", I).each(function() {
								x = this.getBoundingClientRect().width, D += x, S = x > S ?
									x : S
							}), Y.css({
								width: D > g ? S : D,
								"white-space": D > g ? "" : "nowrap"
							})), K = et.offsetWidth, G = et.offsetHeight, ct.scrollLock = it = v >=
							G && g >= K, X && (C = E.scrollLeft(), M = E.scrollTop()), "center" ==
							st.display ? (y = Math.max(0, C + (g - K) / 2), T = Math.max(0, M + (v -
								G) / 2)) : "bubble" == st.display ? (t = st.anchor === s ? mt : o(st
									.anchor), u = o(".mbsc-fr-arr-i", I)[0], l = t.offset(), c = l
								.top + (J ? M - N.offset().top : 0), d = l.left + (J ? C - N
								.offset().left : 0), n = t[0].offsetWidth, i = t[0].offsetHeight,
								h = u.offsetWidth, f = u.offsetHeight, y = m(d - (K - n) / 2, C + 8,
									C + g - K - 8), T = c - G - f / 2, M > T || c > M + v ? (P
									.removeClass("mbsc-fr-bubble-top").addClass(
										"mbsc-fr-bubble-bottom"), T = c + i + f / 2) : P
								.removeClass("mbsc-fr-bubble-bottom").addClass(
								"mbsc-fr-bubble-top"), o(".mbsc-fr-arr", I).css({
									left: m(d + n / 2 - (y + (K - h) / 2), 0, h)
								})) : (y = C, T = "top" == st.display ? M : Math.max(0, M + v - G)),
							X && (p = Math.max(T + G, J ? N[0].scrollHeight : o(a).height()), b =
								Math.max(y + K, J ? N[0].scrollWidth : o(a).width()), O.css({
									width: b,
									height: p
								}), st.scroll && "bubble" == st.display && (T + G + 8 > M + v || c >
									M + v || M > c + i) && (at = !0, setTimeout(function() {
									at = !1
								}, 300), E.scrollTop(Math.min(c, T + G - v + 8, p - v)))), w.top =
							T, w.left = y, P.css(w), ot = g, lt = v)))
					}, ct.attachShow = function(e, t) {
						var a, s = o(e),
							n = s.prop("readonly");
						if ("inline" !== st.display) {
							if ((st.showOnFocus || st.showOnTap) && s.is("input,select") && (s.prop("readonly",
									!0).on("mousedown.mbsc", function(e) {
									e.preventDefault()
								}).on("focus.mbsc", function() {
									ct._isVisible && this.blur()
								}), a = o('label[for="' + s.attr("id") + '"]'), a.length || (a = s.closest(
									"label"))), s.is("select")) return;
							st.showOnFocus && s.on("focus.mbsc", function() {
								i || D(t, s)
							}), st.showOnTap && (s.on("keydown.mbsc", function(e) {
								(32 == e.keyCode || 13 == e.keyCode) && (e.preventDefault(), e
									.stopPropagation(), D(t, s))
							}), ct.tap(s, function() {
								D(t, s)
							}), a && a.length && ct.tap(a, function() {
								D(t, s)
							})), dt.push({
								readOnly: n,
								el: s,
								lbl: a
							})
						}
					}, ct.select = function() {
						R ? ct.hide(!1, "set", !1, k) : k()
					}, ct.cancel = function() {
						R ? ct.hide(!1, "cancel", !1, _) : _()
					}, ct.clear = function() {
						ct._clearValue(), $("onClear"), R && ct._isVisible && !ct.live ? ct.hide(!1, "clear", !
							1, A) : A()
					}, ct.enable = function() {
						st.disabled = !1, ct._isInput && mt.prop("disabled", !1)
					}, ct.disable = function() {
						st.disabled = !0, ct._isInput && mt.prop("disabled", !0)
					}, ct.show = function(t, a) {
						var n, i;
						if (!st.disabled && !ct._isVisible) {
							if (ct._readValue(), $("onBeforeShow") === !1) return !1;
							if (W = st.animate, X = J || "bubble" == st.display, Z = h && !X, n = z.length > 0,
								W !== !1 && ("top" == st.display ? W = "slidedown" : "bottom" == st.display ?
									W = "slideup" : ("center" == st.display || "bubble" == st.display) && (W =
										st.animate || "pop")), R && (q = "mbsc-fr-lock" + (Z ?
										" mbsc-fr-lock-ios" : "") + (J ? " mbsc-fr-lock-ctx" : ""), rt = E
									.scrollTop(), nt = E.scrollLeft(), ot = 0, lt = 0, Z && (L.scrollTop(0), N
										.css({
											top: -rt + "px",
											left: -nt + "px"
										})), L.addClass(q), S(), r.activeInstance && r.activeInstance.hide(), r
									.activeInstance = ct), i = '<div lang="' + st.lang +
								'" class="mbsc-fr mbsc-' + st.theme + (st.baseTheme ? " mbsc-" + st.baseTheme :
									"") + " mbsc-fr-" + st.display + " " + (st.cssClass || "") + " " + (st
									.compClass || "") + (ct._isLiquid ? " mbsc-fr-liq" : "") + (Z ?
									" mbsc-platform-ios" : "") + (n ? z.length >= 3 ? " mbsc-fr-btn-block " :
									"" : " mbsc-fr-nobtn") + '">' + (R ?
									'<div class="mbsc-fr-persp"><div class="mbsc-fr-overlay"></div><div role="dialog" tabindex="-1" class="mbsc-fr-scroll">' :
									"") + '<div class="mbsc-fr-popup' + (st.rtl ? " mbsc-rtl" : " mbsc-ltr") + (
									st.headerText ? " mbsc-fr-has-hdr" : "") + '">' + ("bubble" === st.display ?
									'<div class="mbsc-fr-arr-w"><div class="mbsc-fr-arr-i"><div class="mbsc-fr-arr"></div></div></div>' :
									"") +
								'<div class="mbsc-fr-w"><div aria-live="assertive" class="mbsc-fr-aria mbsc-fr-hdn"></div>' +
								(st.headerText ? '<div class="mbsc-fr-hdr">' + (d(st.headerText) ? st
									.headerText : "") + "</div>" : "") + '<div class="mbsc-fr-c">', i += ct
								._generateContent(), i += "</div>", n && (i += '<div class="mbsc-fr-btn-cont">',
									o.each(z, function(e, t) {
										t = d(t) ? ct.buttons[t] : t, "set" === t.handler && (t
												.parentClass = "mbsc-fr-btn-s"), "cancel" === t.handler && (
												t.parentClass = "mbsc-fr-btn-c"), i += "<div" + (st
												.btnWidth ? ' style="width:' + 100 / z.length + '%"' : "") +
											' class="mbsc-fr-btn-w ' + (t.parentClass || "") +
											'"><div tabindex="0" role="button" class="mbsc-fr-btn' + e +
											" mbsc-fr-btn-e " + (t.cssClass === s ? st.btnClass : t
												.cssClass) + (t.icon ? " mbsc-ic mbsc-ic-" + t.icon : "") +
											'">' + (t.text || "") + "</div></div>"
									}), i += "</div>"), i += "</div></div></div></div>" + (R ? "</div></div>" :
									""), I = o(i), O = o(".mbsc-fr-persp", I), H = o(".mbsc-fr-scroll", I), Y =
								o(".mbsc-fr-w", I), F = o(".mbsc-fr-hdr", I), P = o(".mbsc-fr-popup", I), V = o(
									".mbsc-fr-aria", I), U = I[0], Q = H[0], et = P[0], ct._markup = I, ct
								._header = F, ct._isVisible = !0, tt = "orientationchange resize", ct
								._markupReady(I), $("onMarkupReady", {
									target: U
								}), R) {
								if (o(e).on("keydown", T), st.scrollLock && I.on("touchmove mousewheel wheel",
										function(e) {
											it && e.preventDefault()
										}), st.focusTrap && E.on("focusin", M), st.closeOnOverlayTap) {
									var l, m, f, b;
									H.on("touchstart mousedown", function(e) {
										m || e.target != H[0] || (m = !0, l = !1, f = u(e, "X"), b = u(
											e, "Y"))
									}).on("touchmove mousemove", function(e) {
										m && !l && (Math.abs(u(e, "X") - f) > 9 || Math.abs(u(e, "Y") -
											b) > 9) && (l = !0)
									}).on("touchcancel", function() {
										m = !1
									}).on("touchend touchcancel mouseup", function(e) {
										m && !l && (ct.cancel(), "mouseup" != e.type && c
										.preventClick()), m = !1
									})
								}
								X && (tt += " scroll")
							}
							setTimeout(function() {
								R ? I.appendTo(N) : mt.is("div") && !ct._hasContent ? mt.empty().append(
										I) : I.insertAfter(mt), B = !0, ct._markupInserted(I), $(
										"onMarkupInserted", {
											target: U
										}), I.on("selectstart mousedown", v).on("click",
										".mbsc-fr-btn-e", v).on("keydown", ".mbsc-fr-btn-e", function(
									e) {
										32 == e.keyCode && (e.preventDefault(), e.stopPropagation(),
											this.click())
									}).on("keydown", function(e) {
										if (32 == e.keyCode) e.preventDefault();
										else if (9 == e.keyCode && R && st.focusTrap) {
											var t = I.find('[tabindex="0"]').filter(function() {
													return this.offsetWidth > 0 || this
														.offsetHeight > 0
												}),
												a = t.index(o(":focus", I)),
												s = t.length - 1,
												n = 0;
											e.shiftKey && (s = 0, n = -1), a === s && (t.eq(n)[0]
												.focus(), e.preventDefault())
										}
									}).on("touchstart mousedown pointerdown", ".mbsc-fr-btn-e", g).on(
										"touchend", ".mbsc-fr-btn-e", x), o("input,select,textarea", I)
									.on("selectstart mousedown", function(e) {
										e.stopPropagation()
									}).on("keydown", function(e) {
										32 == e.keyCode && e.stopPropagation()
									}), o.each(z, function(e, t) {
										ct.tap(o(".mbsc-fr-btn" + e, I), function(e) {
											t = d(t) ? ct.buttons[t] : t, (d(t.handler) ? ct
												.handlers[t.handler] : t.handler).call(
												this, e, ct)
										}, !0)
									}), ct._attachEvents(I), ct.position(), E.on(tt, C), R && (W && !t ?
										I.addClass("mbsc-anim-in mbsc-anim-trans mbsc-anim-trans-" + W)
										.on(p, function() {
											I.off(p).removeClass(
												"mbsc-anim-in mbsc-anim-trans mbsc-anim-trans-" +
												W).find(".mbsc-fr-popup").removeClass(
												"mbsc-anim-" + W), y(a)
										}).find(".mbsc-fr-popup").addClass("mbsc-anim-" + W) : y(a)), $(
										"onShow", {
											target: U,
											valueText: ct._tempValue
										})
							}, Z ? 100 : 0)
						}
					}, ct.hide = function(t, a, s, n) {
						return !ct._isVisible || !s && !ct._isValid && "set" == a || !s && $("onBeforeClose", {
							valueText: ct._tempValue,
							button: a
						}) === !1 ? !1 : (I && (R && W && !t && !I.hasClass("mbsc-anim-trans") ? I.addClass(
								"mbsc-anim-out mbsc-anim-trans mbsc-anim-trans-" + W).on(p, function() {
								I.off(p), w(t)
							}).find(".mbsc-fr-popup").addClass("mbsc-anim-" + W) : w(t), ct
							._detachEvents(I), E.off(tt, C).off("focusin", M)), R && (S(), o(e).off(
							"keydown", T), delete r.activeInstance), n && n(), void $("onClose", {
							valueText: ct._value
						}))
					}, ct.ariaMessage = function(e) {
						V.html(""), setTimeout(function() {
							V.html(e)
						}, 100)
					}, ct.isVisible = function() {
						return ct._isVisible
					}, ct.setVal = b, ct.getVal = b, ct._generateContent = b, ct._attachEvents = b, ct
					._detachEvents = b, ct._readValue = b, ct._clearValue = b, ct._fillValue = b, ct
					._markupReady = b, ct._markupInserted = b, ct._markupRemove = b, ct._processSettings = b, ct
					._presetLoad = function(e) {
						e.buttons = e.buttons || ("inline" !== e.display ? ["set", "cancel"] : []), e
							.headerText = e.headerText === s ? "inline" !== e.display ? "{value}" : !1 : e
							.headerText
					}, ct.destroy = function() {
						ct.hide(!0, !1, !0), o.each(dt, function(e, t) {
							t.el.off(".mbsc").prop("readonly", t.readOnly), t.lbl && t.lbl.off(".mbsc")
						}), ct._destroy()
					}, ct.init = function(t) {
						ct._isVisible && ct.hide(!0, !1, !0), ct._init(t), ct._isLiquid = "liquid" === (st
								.layout || (/top|bottom/.test(st.display) ? "liquid" : "")), ct
							._processSettings(), mt.off(".mbsc"), z = st.buttons || [], R = "inline" !== st
							.display, J = "body" != st.context, ct._window = E = o(J ? st.context : e), ct
							._context = N = o(st.context), L = J ? N : o("body,html"), ct.live = !0, o.each(z,
								function(e, t) {
									return "ok" == t || "set" == t || "set" == t.handler ? (ct.live = !1, !1) :
										void 0
								}), ct.buttons.set = {
								text: st.setText,
								handler: "set"
							}, ct.buttons.cancel = {
								text: ct.live ? st.closeText : st.cancelText,
								handler: "cancel"
							}, ct.buttons.clear = {
								text: st.clearText,
								handler: "clear"
							}, ct._isInput = mt.is("input"), $("onInit"), R ? (ct._readValue(), ct
								._hasContent || ct.attachShow(mt)) : ct.show(), mt.on("change.mbsc",
						function() {
								ct._preventChange || ct.setVal(mt.val(), !0, !1), ct._preventChange = !1
							})
					}, ct.buttons = {}, ct.handlers = {
						set: ct.select,
						cancel: ct.cancel,
						clear: ct.clear
					}, ct._value = null, ct._isValid = !0, ct._isVisible = !1, st = ct.settings, $ = ct.trigger,
					f || ct.init(l)
			}, r.classes.Frame.prototype._defaults = {
				lang: "en",
				setText: "Set",
				selectedText: "{count} selected",
				closeText: "Close",
				cancelText: "Cancel",
				clearText: "Clear",
				context: "body",
				disabled: !1,
				closeOnOverlayTap: !0,
				showOnFocus: !1,
				showOnTap: !0,
				display: "center",
				scroll: !0,
				scrollLock: !0,
				tap: !0,
				btnClass: "mbsc-fr-btn",
				btnWidth: !0,
				focusTrap: !0,
				focusOnClose: !f
			}, r.themes.frame.mobiscroll = {
				rows: 5,
				showLabel: !1,
				headerText: !1,
				btnWidth: !1,
				selectedLineBorder: 1,
				weekDays: "min",
				checkIcon: "ion-ios7-checkmark-empty",
				btnPlusClass: "mbsc-ic mbsc-ic-arrow-down5",
				btnMinusClass: "mbsc-ic mbsc-ic-arrow-up5",
				btnCalPrevClass: "mbsc-ic mbsc-ic-arrow-left5",
				btnCalNextClass: "mbsc-ic mbsc-ic-arrow-right5"
			}, o(e).on("focus", function() {
				n && (i = !0)
			})
		}(window, document),
		function() {
			t.themes.frame["android-holo"] = {
				dateDisplay: "Mddyy",
				rows: 5,
				minWidth: 76,
				height: 36,
				showLabel: !1,
				selectedLineBorder: 2,
				useShortLabels: !0,
				icon: {
					filled: "star3",
					empty: "star"
				},
				btnPlusClass: "mbsc-ic mbsc-ic-arrow-down6",
				btnMinusClass: "mbsc-ic mbsc-ic-arrow-up6"
			}
		}(),
		function() {
			t.themes.frame.ios = {
				display: "bottom",
				dateDisplay: "MMdyy",
				rows: 5,
				height: 34,
				minWidth: 55,
				scroll3d: !0,
				headerText: !1,
				showLabel: !1,
				btnWidth: !1,
				selectedLineBorder: 1,
				useShortLabels: !0,
				deleteIcon: "ios-backspace",
				checkIcon: "ion-ios7-checkmark-empty",
				btnCalPrevClass: "mbsc-ic mbsc-ic-arrow-left5",
				btnCalNextClass: "mbsc-ic mbsc-ic-arrow-right5",
				btnPlusClass: "mbsc-ic mbsc-ic-arrow-down5",
				btnMinusClass: "mbsc-ic mbsc-ic-arrow-up5"
			}
		}(),
		function() {
			var e = t,
				a = e.$;
			e.themes.frame.jqm = {
				jqmBody: "a",
				jqmBorder: "a",
				jqmLine: "b",
				jqmSet: "b",
				jqmCancel: "c",
				dateDisplay: "Mddyy",
				disabledClass: "ui-disabled",
				activeClass: "ui-btn-active",
				activeTabInnerClass: "ui-btn-active",
				btnCalPrevClass: "",
				btnCalNextClass: "",
				selectedLineBorder: 1,
				checkIcon: "none ui-btn-icon-left ui-icon-check",
				onThemeLoad: function(e) {
					var t = e.settings,
						a = t.jqmBody || "c",
						s = t.jqmEventBubble || "a";
					t.dayClass = "ui-body-a ui-body-" + a, t.innerDayClass =
						"ui-state-default ui-btn ui-btn-up-" + a, t.calendarClass = "ui-body-a ui-body-" +
						a, t.weekNrClass = "ui-body-a ui-body-" + a, t.eventBubbleClass = "ui-body-" + s
				},
				onInit: function() {
					a(this).closest(".ui-field-contain").trigger("create")
				},
				onEventBubbleShow: function(e) {
					a(".mbsc-cal-event-list", e.eventList).attr("data-role", "listview"), a(e.eventList)
						.page().trigger("create")
				},
				onMarkupInserted: function(e, t) {
					var s = t.settings,
						n = a(e.target);
					a(".mbsc-np-btn, .mbsc-cal-sc-m-cell .mbsc-cal-sc-cell-i", n).addClass("ui-btn"), a(
							".mbsc-fr-btn-cont .mbsc-fr-btn, .mbsc-range-btn", n).addClass(
							"ui-btn ui-mini ui-corner-all"), a(".mbsc-cal-prev .mbsc-cal-btn-txt", n)
						.addClass("ui-btn ui-icon-arrow-l ui-btn-icon-notext ui-shadow ui-corner-all"), a(
							".mbsc-cal-next .mbsc-cal-btn-txt", n).addClass(
							"ui-btn ui-icon-arrow-r ui-btn-icon-notext ui-shadow ui-corner-all"), a(
							".mbsc-fr-popup", n).removeClass("dwbg").addClass(
							"ui-selectmenu ui-overlay-shadow ui-corner-all ui-body-" + s.jqmBorder), a(
							".mbsc-fr-btn-s .mbsc-fr-btn", n).addClass("ui-btn-" + s.jqmSet), a(
							".mbsc-fr-hdr", n).addClass("ui-header ui-bar-inherit"), a(".mbsc-fr-w", n)
						.addClass("ui-corner-all ui-body-" + s.jqmBody), a(".mbsc-sc-btn", n).addClass(
							"ui-btn ui-mini ui-corner-all ui-btn-icon-top"), a(".mbsc-sc-btn-plus", n)
						.addClass("ui-icon-carat-d"), a(".mbsc-sc-btn-minus", n).addClass(
						"ui-icon-carat-u"), a(".mbsc-sc-whl-l", n).addClass("ui-body-" + s.jqmLine), a(
							".mbsc-cal-tabs", n).attr("data-role", "navbar"), a(
							".mbsc-cal-prev .mbsc-cal-btn-txt", n).attr("data-role", "button").attr(
							"data-icon", "arrow-l").attr("data-iconpos", "notext"), a(
							".mbsc-cal-next .mbsc-cal-btn-txt", n).attr("data-role", "button").attr(
							"data-icon", "arrow-r").attr("data-iconpos", "notext"), a(".mbsc-cal-events", n)
						.attr("data-role", "page"), a(".mbsc-range-btn", n).attr("data-role", "button")
						.attr("data-mini", "true"), a(".mbsc-np-btn", n).attr("data-role", "button").attr(
							"data-corners", "false"), n.trigger("create")
				}
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.themes.frame,
				n = {
					minWidth: 76,
					height: 76,
					dateDisplay: "mmMMddDDyy",
					headerText: !1,
					showLabel: !1,
					deleteIcon: "backspace4",
					icon: {
						filled: "star3",
						empty: "star"
					},
					btnWidth: !1,
					btnCalPrevClass: "mbsc-ic mbsc-ic-arrow-left2",
					btnCalNextClass: "mbsc-ic mbsc-ic-arrow-right2",
					btnPlusClass: "mbsc-ic mbsc-ic-plus",
					btnMinusClass: "mbsc-ic mbsc-ic-minus",
					onMarkupInserted: function(e, t) {
						function s(e) {
							return a.isArray(l.readonly) ? l.readonly[e] : l.readonly
						}
						var n, i, r, o = a(e.target),
							l = t.settings;
						a(".mbsc-sc-whl", o).on("touchstart mousedown wheel mousewheel", function(e) {
							"mousedown" === e.type && i || s(a(this).attr("data-index")) || (i =
								"touchstart" === e.type, n = !0, r = a(this).hasClass(
									"mbsc-sc-whl-wpa"), a(".mbsc-sc-whl", o).removeClass(
									"mbsc-sc-whl-wpa"), a(this).addClass("mbsc-sc-whl-wpa"))
						}).on("touchmove mousemove", function() {
							n = !1
						}).on("touchend mouseup", function(e) {
							n && r && a(e.target).closest(".mbsc-sc-itm").hasClass("mbsc-sc-itm-sel") &&
								a(this).removeClass("mbsc-sc-whl-wpa"), "mouseup" === e.type && (i = !
								1), n = !1
						})
					},
					onInit: function(e, t) {
						var a = t.buttons;
						a.set.icon = "checkmark", a.cancel.icon = "close", a.clear.icon = "close", a.ok && (a.ok
							.icon = "checkmark"), a.close && (a.close.icon = "close"), a.now && (a.now
							.icon = "loop2"), a.toggle && (a.toggle.icon = "play3"), a.start && (a.start
							.icon = "play3"), a.stop && (a.stop.icon = "pause2"), a.reset && (a.reset.icon =
							"stop2"), a.lap && (a.lap.icon = "loop2"), a.hide && (a.hide.icon = "close")
					}
				};
			s.wp = n
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = a.classes,
				i = a.util,
				r = i.constrain,
				o = i.jsPrefix,
				l = i.prefix,
				c = i.getCoord,
				m = i.getPosition,
				d = i.testTouch,
				u = i.isNumeric,
				h = i.isString,
				f = /(iphone|ipod|ipad)/i.test(navigator.userAgent),
				p = function() {},
				b = window.requestAnimationFrame || function(e) {
					e()
				},
				v = window.cancelAnimationFrame || p;
			n.ScrollView = function(a, i, p) {
				function g(e) {
					it("onStart"), dt.stopProp && e.stopPropagation(), (dt.prevDef || "mousedown" == e.type) &&
						e.preventDefault(), dt.readonly || dt.lock && $ || d(e, this) && !W && t.vKMaI && (S &&
							S.removeClass("mbsc-btn-a"), P = !1, $ || (S = s(e.target).closest(".mbsc-btn-e",
								this), S.length && !S.hasClass("mbsc-btn-d") && (P = !0, D = setTimeout(
								function() {
									S.addClass("mbsc-btn-a")
								}, 100))), W = !0, q = !1, J = !1, lt.scrolled = $, et = c(e, "X"), tt = c(e,
								"Y"), I = E = et, _ = 0, A = 0, V = 0, Q = new Date, Z = +m(st, rt) || 0, $ &&
							M(Z, f ? 0 : 1), "mousedown" === e.type && s(document).on("mousemove", x).on(
								"mouseup", y))
				}

				function x(e) {
					W && (dt.stopProp && e.stopPropagation(), I = c(e, "X"), H = c(e, "Y"), _ = I - et, A = H -
						tt, V = rt ? A : _, P && (Math.abs(A) > 5 || Math.abs(_) > 5) && (clearTimeout(D), S
							.removeClass("mbsc-btn-a"), P = !1), (lt.scrolled || !J && Math.abs(V) > 5) && (
							q || it("onGestureStart", O), lt.scrolled = q = !0, B || (B = !0, R = b(T))),
						rt || dt.scrollLock ? e.preventDefault() : lt.scrolled ? e.preventDefault() : Math
						.abs(A) > 7 && (J = !0, lt.scrolled = !0, ut.trigger("touchend")))
				}

				function T() {
					z && (V = r(V, -G * z, G * z)), M(r(Z + V, j - L, Y + L)), B = !1
				}

				function y(e) {
					if (W) {
						var t, a = new Date - Q;
						dt.stopProp && e.stopPropagation(), v(R), B = !1, !J && lt.scrolled && (dt.momentum &&
								300 > a && (t = V / a, V = Math.max(Math.abs(V), t * t / dt.speedUnit) * (0 >
									V ? -1 : 1)), C(V)), P && (clearTimeout(D), S.addClass("mbsc-btn-a"),
								setTimeout(function() {
									S.removeClass("mbsc-btn-a")
								}, 100), J || lt.scrolled || it("onBtnTap", {
									target: S[0]
								})), "mouseup" == e.type && s(document).off("mousemove", x).off("mouseup", y),
							W = !1
					}
				}

				function w(e) {
					if (e = e.originalEvent || e, V = rt ? e.deltaY || e.wheelDelta || e.detail : e.deltaX, it(
							"onStart"), dt.stopProp && e.stopPropagation(), V) {
						if (e.preventDefault(), dt.readonly) return;
						V = 0 > V ? 20 : -20, Z = ot, q || (O = {
								posX: rt ? 0 : ot,
								posY: rt ? ot : 0,
								originX: rt ? 0 : Z,
								originY: rt ? Z : 0,
								direction: V > 0 ? rt ? 270 : 360 : rt ? 90 : 180
							}, it("onGestureStart", O)), B || (B = !0, R = b(T)), q = !0, clearTimeout(U), U =
							setTimeout(function() {
								v(R), B = !1, q = !1, C(V)
							}, 200)
					}
				}

				function C(e) {
					var t, a, s;
					if (z && (e = r(e, -G * z, G * z)), ct = Math.round((Z + e) / G), s = r(ct * G, j, Y), X) {
						if (0 > e) {
							for (t = X.length - 1; t >= 0; t--)
								if (Math.abs(s) + k >= X[t].breakpoint) {
									ct = t, mt = 2, s = X[t].snap2;
									break
								}
						} else if (e >= 0)
							for (t = 0; t < X.length; t++)
								if (Math.abs(s) <= X[t].breakpoint) {
									ct = t, mt = 1, s = X[t].snap1;
									break
								} s = r(s, j, Y)
					}
					a = dt.time || (j > ot || ot > Y ? 1e3 : Math.max(1e3, Math.abs(s - ot) * dt.timeUnit)), O
						.destinationX = rt ? 0 : s, O.destinationY = rt ? s : 0, O.duration = a, O
						.transitionTiming = F, it("onGestureEnd", O), M(s, a)
				}

				function M(e, t, a, s) {
					var n = e != ot,
						i = t > 1,
						r = function() {
							clearInterval(K), clearTimeout(nt), $ = !1, ot = e, O.posX = rt ? 0 : e, O.posY =
								rt ? e : 0, n && it("onMove", O), i && it("onAnimationEnd", O), s && s()
						};
					O = {
							posX: rt ? 0 : ot,
							posY: rt ? ot : 0,
							originX: rt ? 0 : Z,
							originY: rt ? Z : 0,
							direction: e - ot > 0 ? rt ? 270 : 360 : rt ? 90 : 180
						}, ot = e, i && (O.destinationX = rt ? 0 : e, O.destinationY = rt ? e : 0, O.duration =
							t, O.transitionTiming = F, it("onAnimationStart", O)), at[o + "Transition"] = t ?
						l + "transform " + Math.round(t) + "ms " + F : "", at[o + "Transform"] =
						"translate3d(" + (rt ? "0," + e + "px," : e + "px,0,") + "0)", !n && !$ || !t || 1 >=
						t ? r() : t && ($ = !a, clearInterval(K), K = setInterval(function() {
							var t = +m(st, rt) || 0;
							O.posX = rt ? 0 : t, O.posY = rt ? t : 0, it("onMove", O), Math.abs(t - e) <
								2 && r()
						}, 100), clearTimeout(nt), nt = setTimeout(function() {
							r()
						}, t)), dt.sync && dt.sync(e, t, F)
				}
				var S, D, k, _, A, V, N, F, L, I, H, O, P, E, Y, z, j, W, $, J, R, B, q, U, K, G, X, Z, Q, et,
					tt, at, st, nt, it, rt, ot, lt = this,
					ct = 0,
					mt = 1,
					dt = i,
					ut = s(a);
				n.Base.call(this, a, i, !0), lt.scrolled = !1, lt.scroll = function(e, t, n, i) {
					e = u(e) ? Math.round(e / G) * G : Math.ceil((s(e, a).length ? Math.round(st.offset()[
						N] - s(e, a).offset()[N]) : ot) / G) * G, ct = Math.round(e / G), Z = ot, M(r(e,
						j, Y), t, n, i)
				}, lt.refresh = function(t) {
					var a;
					k = dt.contSize === e ? rt ? ut.height() : ut.width() : dt.contSize, j = dt
						.minScroll === e ? rt ? k - st.height() : k - st.width() : dt.minScroll, Y = dt
						.maxScroll === e ? 0 : dt.maxScroll, X = null, !rt && dt.rtl && (a = Y, Y = -j,
							j = -a), h(dt.snap) && (X = [], st.find(dt.snap).each(function() {
							var e = rt ? this.offsetTop : this.offsetLeft,
								t = rt ? this.offsetHeight : this.offsetWidth;
							X.push({
								breakpoint: e + t / 2,
								snap1: -e,
								snap2: k - e - t
							})
						})), G = u(dt.snap) ? dt.snap : 1, z = dt.snap ? dt.maxSnapScroll : 0, F = dt
						.easing, L = dt.elastic ? u(dt.snap) ? G : u(dt.elastic) ? dt.elastic : 0 : 0,
						ot === e && (ot = dt.initialPos, ct = Math.round(ot / G)), t || lt.scroll(dt.snap ?
							X ? X[ct]["snap" + mt] : ct * G : ot)
				}, lt.init = function(e) {
					lt._init(e), rt = "Y" == dt.axis, N = rt ? "top" : "left", st = dt.moveElement || ut
						.children().eq(0), at = st[0].style, lt.refresh(), ut.on("touchstart mousedown", g)
						.on("touchmove", x).on("touchend touchcancel", y), dt.mousewheel && ut.on(
							"wheel mousewheel", w), a.addEventListener && a.addEventListener("click",
							function(e) {
								lt.scrolled && (lt.scrolled = !1, e.stopPropagation(), e.preventDefault())
							}, !0)
				}, lt.destroy = function() {
					clearInterval(K), ut.off("touchstart mousedown", g).off("touchmove", x).off(
						"touchend touchcancel", y).off("wheel mousewheel", w), lt._destroy()
				}, dt = lt.settings, it = lt.trigger, p || lt.init(i)
			}, n.ScrollView.prototype = {
				_class: "scrollview",
				_defaults: {
					speedUnit: .0022,
					timeUnit: 3,
					initialPos: 0,
					axis: "Y",
					easing: "cubic-bezier(0.190, 1.000, 0.220, 1.000)",
					stopProp: !0,
					momentum: !0,
					mousewheel: !0,
					elastic: !0
				}
			}, a.presetShort("scrollview", "ScrollView", !1)
		}(),
		function(e, a, s) {
			var n = t,
				i = n.$,
				r = i.extend,
				o = n.classes,
				l = n.platform,
				c = n.util,
				m = c.jsPrefix,
				d = c.prefix,
				u = c.getCoord,
				h = c.testTouch,
				f = "wp" == l.name || "android" == l.name || "ios" == l.name && l.majorVersion < 8;
			n.presetShort("scroller", "Scroller", !1), o.Scroller = function(e, l, p) {
				function b(e) {
					var t = i(this).attr("data-index");
					e.stopPropagation(), "mousedown" === e.type && e.preventDefault(), h(e, this) && !V(t) && (
						z = i(this).addClass("mbsc-sc-btn-a"), K = u(e, "X"), G = u(e, "Y"), q = !0, U = !1,
						setTimeout(function() {
							k(t, "inc" == z.attr("data-dir") ? 1 : -1)
						}, 100), "mousedown" === e.type && i(a).on("mousemove", v).on("mouseup", g))
				}

				function v(e) {
					(Math.abs(K - u(e, "X")) > 7 || Math.abs(G - u(e, "Y")) > 7) && _(!0)
				}

				function g(e) {
					_(), e.preventDefault(), "mouseup" === e.type && i(a).off("mousemove", v).off("mouseup", g)
				}

				function x(e) {
					var t, a, s = i(this).attr("data-index");
					38 == e.keyCode ? (t = !0, a = -1) : 40 == e.keyCode ? (t = !0, a = 1) : 32 == e.keyCode &&
						(t = !0, D(s)), t && (e.stopPropagation(), e.preventDefault(), a && !q && (q = !0, U = !
							1, k(s, a)))
				}

				function T() {
					_()
				}

				function y(e, t) {
					return (e._array ? e._map[t] : e.getIndex(t)) || 0
				}

				function w(e, t) {
					var a = e.data;
					return t >= e.min && t <= e.max ? e._array ? e.circular ? i(a).get(t % e._length) : a[t] : i
						.isFunction(a) ? a(t) : "" : void 0
				}

				function C(e) {
					return i.isPlainObject(e) ? e.value !== s ? e.value : e.display : e
				}

				function M(e) {
					var t = i.isPlainObject(e) ? e.display : e;
					return t === s ? "" : t
				}

				function S(e, t) {
					return C(w(e, t))
				}

				function D(e, t) {
					var a = nt[e],
						n = t || a._$markup.find('.mbsc-sc-itm[data-val="' + X[e] + '"]'),
						i = +n.attr("data-index"),
						r = S(a, i),
						o = lt._tempSelected[e],
						l = c.isNumeric(a.multiple) ? a.multiple : 1 / 0;
					return a.multiple && !a._disabled[r] ? (o[r] !== s ? (n.removeClass(J).removeAttr(
						"aria-selected"), delete o[r]) : c.objectToArray(o).length < l && (n.addClass(J)
						.attr("aria-selected", "true"), o[r] = r), !0) : void 0
				}

				function k(e, a) {
					U || A(e, a), q && t.vKMaI && (clearInterval(B), B = setInterval(function() {
						A(e, a)
					}, tt.delay))
				}

				function _(e) {
					clearInterval(B), U = e, q = !1, z && z.removeClass("mbsc-sc-btn-a")
				}

				function A(e, t) {
					var a = nt[e];
					P(a, e, a._current + t, ot, 1 == t ? 1 : 2)
				}

				function V(e) {
					return i.isArray(tt.readonly) ? tt.readonly[e] : tt.readonly
				}

				function N(e, t, a) {
					var n = e._index - e._batch;
					return e.data = e.data || [], e.key = e.key !== s ? e.key : t, e.label = e.label !== s ? e
						.label : t, e._map = {}, e._array = i.isArray(e.data), e._array && (e._length = e.data
							.length, i.each(e.data, function(t, a) {
								e._map[C(a)] = t
							})), e.circular = tt.circular === s ? e.circular === s ? e._array && e._length > tt
						.rows : e.circular : i.isArray(tt.circular) ? tt.circular[t] : tt.circular, e.min = e
						._array ? e.circular ? -1 / 0 : 0 : e.min === s ? -1 / 0 : e.min, e.max = e._array ? e
						.circular ? 1 / 0 : e._length - 1 : e.max === s ? 1 / 0 : e.max, e._nr = t, e._index =
						y(e, X[t]), e._disabled = {}, e._batch = 0, e._current = e._index, e._first = e._index -
						rt, e._last = e._index + rt, e._offset = e._first, a ? (e._offset -= e._margin / Z + (e
							._index - n), e._margin += (e._index - n) * Z) : e._margin = 0, e._refresh =
						function(t) {
							var a = -(e.min - e._offset + (e.multiple && !$ ? Math.floor(tt.rows / 2) : 0)) * Z,
								s = Math.min(a, -(e.max - e._offset - (e.multiple && !$ ? Math.floor(tt.rows /
									2) : 0)) * Z);
							r(e._scroller.settings, {
								minScroll: s,
								maxScroll: a
							}), e._scroller.refresh(t)
						}, it[e.key] = e, e
				}

				function F(e, t, a, n, i) {
					var r, o, l, c, m, u, h, f, p = "",
						b = lt._tempSelected[t],
						v = e._disabled || {};
					for (r = a; n >= r; r++) l = w(e, r), m = M(l), c = C(l), o = l && l.cssClass !== s ? l
						.cssClass : "", u = l && l.label !== s ? l.label : "", h = l && l.invalid, f = c !==
						s && c == X[t] && !e.multiple, p += '<div role="option" aria-selected="' + (b[c] ? !0 :
							!1) + '" class="mbsc-sc-itm ' + (i ? "mbsc-sc-itm-3d " : "") + o + " " + (f ?
							"mbsc-sc-itm-sel " : "") + (b[c] ? J : "") + (c === s ? " mbsc-sc-itm-ph" :
							" mbsc-btn-e") + (h ? " mbsc-sc-itm-inv-h mbsc-btn-d" : "") + (v[c] ?
							" mbsc-sc-itm-inv mbsc-btn-d" : "") + '" data-index="' + r + '" data-val="' + c +
						'"' + (u ? ' aria-label="' + u + '"' : "") + (f ? ' aria-selected="true"' : "") +
						' style="height:' + Z + "px;line-height:" + Z + "px;" + (i ? d + "transform:rotateX(" +
							(e._offset - r) * W % 360 + "deg) translateZ(" + Z * tt.rows / 2 + "px);" : "") +
						'">' + (st > 1 ? '<div class="mbsc-sc-itm-ml" style="line-height:' + Math.round(Z /
							st) + "px;font-size:" + Math.round(Z / st * .8) + 'px;">' : "") + m + (st > 1 ?
							"</div>" : "") + "</div>";
					return p
				}

				function L(t) {
					var a = tt.headerText;
					return a ? "function" == typeof a ? a.call(e, t) : a.replace(/\{value\}/i, t) : ""
				}

				function I(e, t, a) {
					var s = Math.round(-a / Z) + e._offset,
						n = s - e._current,
						r = e._first,
						o = e._last,
						l = r + rt - j + 1,
						c = o - rt + j;
					n && (e._first += n, e._last += n, e._current = s, n > 0 ? (e._$scroller.append(F(e, t, Math
							.max(o + 1, r + n), o + n)), i(".mbsc-sc-itm", e._$scroller).slice(0, Math
							.min(n, o - r + 1)).remove(), $ && (e._$3d.append(F(e, t, Math.max(c + 1,
							l + n), c + n, !0)), i(".mbsc-sc-itm", e._$3d).slice(0, Math.min(n, c -
							l + 1)).attr("class", "mbsc-sc-itm-del"))) : 0 > n && (e._$scroller.prepend(F(e,
							t, r + n, Math.min(r - 1, o + n))), i(".mbsc-sc-itm", e._$scroller).slice(
							Math.max(n, r - o - 1)).remove(), $ && (e._$3d.prepend(F(e, t, l + n, Math
							.min(l - 1, c + n), !0)), i(".mbsc-sc-itm", e._$3d).slice(Math.max(n,
							l - c - 1)).attr("class", "mbsc-sc-itm-del"))), e._margin += n * Z, e._$scroller
						.css("margin-top", e._margin + "px"))
				}

				function H(e, t, a, n) {
					var i, r = nt[e],
						o = n || r._disabled,
						l = y(r, t),
						c = t,
						m = t,
						d = 0,
						u = 0;
					if (t === s && (t = S(r, l)), o[t]) {
						for (i = 0; l - d >= r.min && o[c] && 100 > i;) i++, d++, c = S(r, l - d);
						for (i = 0; l + u < r.max && o[m] && 100 > i;) i++, u++, m = S(r, l + u);
						t = (d > u && u && 2 !== a || !d || 0 > l - d || 1 == a) && !o[m] ? m : c
					}
					return t
				}

				function O(t, a, n, r, o) {
					var l, c, m, d, u = lt._isVisible;
					et = !0, d = tt.validate.call(e, {
							values: X.slice(0),
							index: a,
							direction: n
						}, lt) || {}, et = !1, d.valid && (lt._tempWheelArray = X = d.valid.slice(0)), at(
							"onValidated"), i.each(nt, function(e, r) {
							if (u && r._$markup.find(".mbsc-sc-itm-inv").removeClass(
									"mbsc-sc-itm-inv mbsc-btn-d"), r._disabled = {}, d.disabled && d
								.disabled[e] && i.each(d.disabled[e], function(e, t) {
									r._disabled[t] = !0, u && r._$markup.find(
										'.mbsc-sc-itm[data-val="' + t + '"]').addClass(
										"mbsc-sc-itm-inv mbsc-btn-d")
								}), X[e] = r.multiple ? X[e] : H(e, X[e], n), u) {
								if (r.multiple && a !== s || r._$markup.find(".mbsc-sc-itm-sel")
									.removeClass(J).removeAttr("aria-selected"), r.multiple) {
									if (a === s)
										for (var h in lt._tempSelected[e]) r._$markup.find(
											'.mbsc-sc-itm[data-val="' + h + '"]').addClass(J).attr(
											"aria-selected", "true")
								} else r._$markup.find('.mbsc-sc-itm[data-val="' + X[e] + '"]').addClass(
									"mbsc-sc-itm-sel").attr("aria-selected", "true");
								c = y(r, X[e]), l = c - r._index + r._batch, Math.abs(l) > 2 * rt + 1 && (
										m = l + (2 * rt + 1) * (l > 0 ? -1 : 1), r._offset += m, r
										._margin -= m * Z, r._refresh()), r._index = c + r._batch, r
									._scroller.scroll(-(c - r._offset + r._batch) * Z, a === e || a === s ?
										t : ot, o)
							}
						}), lt._tempValue = tt.formatValue(X, lt), u && lt._header.html(L(lt._tempValue)), lt
						.live && (lt._hasValue = r || lt._hasValue, E(r, r, 0, !0), r && at("onSet", {
							valueText: lt._value
						})), r && at("onChange", {
							valueText: lt._tempValue
						})
				}

				function P(e, t, a, n, i, r) {
					var o = S(e, a);
					o !== s && (X[t] = o, e._batch = e._array ? Math.floor(a / e._length) * e._length : 0,
						setTimeout(function() {
							O(n, t, i, !0, r)
						}, 10))
				}

				function E(e, t, a, s, n) {
					s ? lt._tempValue = tt.formatValue(lt._tempWheelArray, lt) : O(a), n || (lt._wheelArray = X
						.slice(0), lt._value = lt._hasValue ? lt._tempValue : null, lt._selected = r(!0, {},
							lt._tempSelected)), e && (lt._isInput && ct.val(lt._hasValue ? lt._tempValue :
						""), at("onFill", {
						valueText: lt._hasValue ? lt._tempValue : "",
						change: t
					}), t && (lt._preventChange = !0, ct.trigger("change")))
				}
				var Y, z, j, W, $, J, R, B, q, U, K, G, X, Z, Q, et, tt, at, st, nt, it, rt = 40,
					ot = 1e3,
					lt = this,
					ct = i(e);
				o.Frame.call(this, e, l, !0), lt.setVal = lt._setVal = function(t, a, n, r, o) {
					lt._hasValue = null !== t && t !== s, lt._tempWheelArray = X = i.isArray(t) ? t.slice(
						0) : tt.parseValue.call(e, t, lt) || [], E(a, n === s ? a : n, o, !1, r)
				}, lt.getVal = lt._getVal = function(e) {
					var t = lt._hasValue || e ? lt[e ? "_tempValue" : "_value"] : null;
					return c.isNumeric(t) ? +t : t
				}, lt.setArrayVal = lt.setVal, lt.getArrayVal = function(e) {
					return e ? lt._tempWheelArray : lt._wheelArray
				}, lt.changeWheel = function(e, t, a) {
					var n, o;
					i.each(e, function(e, t) {
						o = it[e], n = o._nr, o && (r(o, t), N(o, n, !0), lt._isVisible && ($ && o
							._$3d.html(F(o, n, o._first + rt - j + 1, o._last - rt + j, !
							0)), o._$scroller.html(F(o, n, o._first, o._last)).css(
								"margin-top", o._margin + "px"), o._refresh(et)))
					}), lt._isVisible && !et && lt.position(), et || O(t, s, s, a)
				}, lt.getValidValue = H, lt._generateContent = function() {
					var e, t = "",
						a = $ ? d + "transform: translateZ(" + (Z * tt.rows / 2 + 3) + "px);" : "",
						n = '<div class="mbsc-sc-whl-l" style="' + a + "height:" + Z + "px;margin-top:-" + (
							Z / 2 + (tt.selectedLineBorder || 0)) + 'px;"></div>',
						o = 0;
					return i.each(tt.wheels, function(l, c) {
						t += '<div class="mbsc-w-p mbsc-sc-whl-gr-c' + (tt.showLabel ?
								" mbsc-sc-lbl-v" : "") + '">' + n + '<div class="mbsc-sc-whl-gr' + (
								$ ? " mbsc-sc-whl-gr-3d" : "") + (R ? " mbsc-sc-cp" : "") + '">', i
							.each(c, function(i, l) {
								lt._tempSelected[o] = r({}, lt._selected[o]), nt[o] = N(l, o),
									e = l.label !== s ? l.label : i, t +=
									'<div class="mbsc-sc-whl-w ' + (l.cssClass || "") + (l
										.multiple ? " mbsc-sc-whl-multi" : "") + '" style="' + (
										tt.width ? "width:" + (tt.width[o] || tt.width) +
										"px;" : (tt.minWidth ? "min-width:" + (tt.minWidth[o] ||
											tt.minWidth) + "px;" : "") + (tt.maxWidth ?
											"max-width:" + (tt.maxWidth[o] || tt.maxWidth) +
											"px;" : "")) +
									'"><div class="mbsc-sc-whl-o" style="' + a + '"></div>' +
									n + '<div tabindex="0" aria-live="off" aria-label="' + e +
									'"' + (l.multiple ? ' aria-multiselectable="true"' : "") +
									' role="listbox" data-index="' + o +
									'" class="mbsc-sc-whl" style="height:' + tt.rows * Z * ($ ?
										1.1 : 1) + 'px;">' + (R ? '<div data-index="' + o +
										'" data-dir="inc" class="mbsc-sc-btn mbsc-sc-btn-plus ' +
										(tt.btnPlusClass || "") + '" style="height:' + Z +
										"px;line-height:" + Z + 'px;"></div><div data-index="' +
										o +
										'" data-dir="dec" class="mbsc-sc-btn mbsc-sc-btn-minus ' +
										(tt.btnMinusClass || "") + '" style="height:' + Z +
										"px;line-height:" + Z + 'px;"></div>' : "") +
									'<div class="mbsc-sc-lbl">' + e +
									'</div><div class="mbsc-sc-whl-c" style="height:' + Q +
									"px;margin-top:-" + (Q / 2 + 1) + "px;" + a +
									'"><div class="mbsc-sc-whl-sc" style="top:' + (Q - Z) / 2 +
									'px;">', t += F(l, o, l._first, l._last) + "</div></div>",
									$ && (t += '<div class="mbsc-sc-whl-3d" style="height:' +
										Z + "px;margin-top:-" + Z / 2 + 'px;">', t += F(l, o, l
											._first + rt - j + 1, l._last - rt + j, !0), t +=
										"</div>"), t += "</div></div>", o++
							}), t += "</div></div>"
					}), t
				}, lt._attachEvents = function(e) {
					i(".mbsc-sc-btn", e).on("touchstart mousedown", b).on("touchmove", v).on(
						"touchend touchcancel", g), i(".mbsc-sc-whl", e).on("keydown", x).on("keyup", T)
				}, lt._detachEvents = function(e) {
					i(".mbsc-sc-whl", e).mobiscroll("destroy")
				}, lt._markupReady = function(e) {
					Y = e, i(".mbsc-sc-whl", Y).each(function(e) {
						var t, a = i(this),
							s = nt[e],
							r = -(s.min - s._offset + (s.multiple && !$ ? Math.floor(tt.rows / 2) :
								0)) * Z,
							o = Math.min(r, -(s.max - s._offset - (s.multiple && !$ ? Math.floor(tt
								.rows / 2) : 0)) * Z);
						s._$markup = a, s._$scroller = i(".mbsc-sc-whl-sc", this), s._$3d = i(
							".mbsc-sc-whl-3d", this), s._scroller = new n.classes.ScrollView(
							this, {
								mousewheel: tt.mousewheel,
								moveElement: s._$scroller,
								initialPos: (s._first - s._index) * Z,
								contSize: 0,
								snap: Z,
								minScroll: o,
								maxScroll: r,
								maxSnapScroll: rt,
								prevDef: !0,
								stopProp: !0,
								timeUnit: 3,
								easing: "cubic-bezier(0.190, 1.000, 0.220, 1.000)",
								sync: function(e, t, a) {
									$ && (s._$3d[0].style[m + "Transition"] = t ? d +
										"transform " + Math.round(t) + "ms " + a : "", s
										._$3d[0].style[m + "Transform"] = "rotateX(" + -
										e / Z * W + "deg)")
								},
								onStart: function(t, a) {
									a.settings.readonly = V(e)
								},
								onGestureStart: function() {
									a.addClass("mbsc-sc-whl-a mbsc-sc-whl-anim"), at(
										"onWheelGestureStart", {
											index: e
										})
								},
								onGestureEnd: function(a) {
									var n = 90 == a.direction ? 1 : 2,
										i = a.duration,
										r = a.destinationY;
									t = Math.round(-r / Z) + s._offset, P(s, e, t, i, n)
								},
								onAnimationStart: function() {
									a.addClass("mbsc-sc-whl-anim")
								},
								onAnimationEnd: function() {
									a.removeClass("mbsc-sc-whl-a mbsc-sc-whl-anim"), at(
										"onWheelAnimationEnd", {
											index: e
										}), s._$3d.find(".mbsc-sc-itm-del").remove()
								},
								onMove: function(t) {
									I(s, e, t.posY)
								},
								onBtnTap: function(t) {
									var a = i(t.target),
										n = +a.attr("data-index");
									D(e, a) && (n = s._current), at("onItemTap", {
										target: a[0],
										selected: a.hasClass("mbsc-itm-sel")
									}) !== !1 && (P(s, e, n, ot, !0, !0), !lt.live || s
										.multiple || tt.setOnTap !== !0 && !tt.setOnTap[
											e] || setTimeout(function() {
											lt.select()
										}, 200))
								}
							})
					}), O()
				}, lt._fillValue = function() {
					lt._hasValue = !0, E(!0, !0, 0, !0)
				}, lt._clearValue = function() {
					i(".mbsc-sc-whl-multi .mbsc-sc-itm-sel", Y).removeClass(J).removeAttr("aria-selected")
				}, lt._readValue = function() {
					var t = ct.val() || "",
						a = 0;
					"" !== t && (lt._hasValue = !0), lt._tempWheelArray = X = lt._hasValue && lt
						._wheelArray ? lt._wheelArray.slice(0) : tt.parseValue.call(e, t, lt) || [], lt
						._tempSelected = r(!0, {}, lt._selected), i.each(tt.wheels, function(e, t) {
							i.each(t, function(e, t) {
								nt[a] = N(t, a), a++
							})
						}), E(!1, !1, 0, !0), at("onRead")
				}, lt._processSettings = function() {
					tt = lt.settings, tt.cssClass = (tt.cssClass || "") + " mbsc-sc", at = lt.trigger, R =
						tt.showScrollArrows, $ = tt.scroll3d && !f && !R, Z = tt.height, Q = $ ? 2 * Math
						.round((Z - .03 * (Z * tt.rows / 2 + 3)) / 2) : Z, st = tt.multiline, J =
						"mbsc-sc-itm-sel mbsc-ic mbsc-ic-" + tt.checkIcon, nt = [], it = {}, j = Math.round(
							1.8 * tt.rows), W = 360 / (2 * j), lt._isLiquid = "liquid" === (tt.layout || (
							/top|bottom/.test(tt.display) && 1 == tt.wheels.length ? "liquid" : "")), st >
						1 && (tt.cssClass = (tt.cssClass || "") + " dw-ml"), R && (tt.rows = Math.max(3, tt
							.rows))
				}, lt._getItemValue = C, lt._tempSelected = {}, lt._selected = {}, p || lt.init(l)
			}, o.Scroller.prototype = {
				_hasDef: !0,
				_hasTheme: !0,
				_hasLang: !0,
				_hasPreset: !0,
				_class: "scroller",
				_defaults: r({}, o.Frame.prototype._defaults, {
					minWidth: 80,
					height: 40,
					rows: 3,
					multiline: 1,
					delay: 300,
					readonly: !1,
					showLabel: !0,
					setOnTap: !1,
					wheels: [],
					preset: "",
					speedUnit: .0012,
					timeUnit: .08,
					validate: function() {},
					formatValue: function(e) {
						return e.join(" ")
					},
					parseValue: function(e, t) {
						var a, n, r = [],
							o = [],
							l = 0;
						return null !== e && e !== s && (r = (e + "").split(" ")), i.each(t.settings
							.wheels,
							function(e, s) {
								i.each(s, function(e, s) {
									n = s.data, a = t._getItemValue(n[0]), i.each(n,
										function(e, s) {
											return r[l] == t._getItemValue(s) ? (a =
												t._getItemValue(s), !1) : void 0
										}), o.push(a), l++
								})
							}), o
					}
				})
			}, n.themes.scroller = n.themes.frame
		}(window, document),
		function() {
			function e(e, t, a, s, n, i, r) {
				var o = new Date(e, t, a, s || 0, n || 0, i || 0, r || 0);
				return 23 == o.getHours() && 0 === (s || 0) && o.setHours(o.getHours() + 2), o
			}
			var a = t,
				s = a.$;
			a.util.datetime = {
				defaults: {
					shortYearCutoff: "+10",
					monthNames: ["January", "February", "March", "April", "May", "June", "July", "August",
						"September", "October", "November", "December"
					],
					monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct",
						"Nov", "Dec"
					],
					dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
					dayNamesShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
					dayNamesMin: ["S", "M", "T", "W", "T", "F", "S"],
					amText: "am",
					pmText: "pm",
					getYear: function(e) {
						return e.getFullYear()
					},
					getMonth: function(e) {
						return e.getMonth()
					},
					getDay: function(e) {
						return e.getDate()
					},
					getDate: e,
					getMaxDayOfMonth: function(e, t) {
						return 32 - new Date(e, t, 32, 12).getDate()
					},
					getWeekNumber: function(e) {
						e = new Date(e), e.setHours(0, 0, 0), e.setDate(e.getDate() + 4 - (e.getDay() ||
						7));
						var t = new Date(e.getFullYear(), 0, 1);
						return Math.ceil(((e - t) / 864e5 + 1) / 7)
					}
				},
				adjustedDate: e,
				formatDate: function(e, t, n) {
					if (!t) return null;
					var i, r, o = s.extend({}, a.util.datetime.defaults, n),
						l = function(t) {
							for (var a = 0; i + 1 < e.length && e.charAt(i + 1) == t;) a++, i++;
							return a
						},
						c = function(e, t, a) {
							var s = "" + t;
							if (l(e))
								for (; s.length < a;) s = "0" + s;
							return s
						},
						m = function(e, t, a, s) {
							return l(e) ? s[t] : a[t]
						},
						d = "",
						u = !1;
					for (i = 0; i < e.length; i++)
						if (u) "'" != e.charAt(i) || l("'") ? d += e.charAt(i) : u = !1;
						else switch (e.charAt(i)) {
							case "d":
								d += c("d", o.getDay(t), 2);
								break;
							case "D":
								d += m("D", t.getDay(), o.dayNamesShort, o.dayNames);
								break;
							case "o":
								d += c("o", (t.getTime() - new Date(t.getFullYear(), 0, 0).getTime()) /
									864e5, 3);
								break;
							case "m":
								d += c("m", o.getMonth(t) + 1, 2);
								break;
							case "M":
								d += m("M", o.getMonth(t), o.monthNamesShort, o.monthNames);
								break;
							case "y":
								r = o.getYear(t), d += l("y") ? r : (10 > r % 100 ? "0" : "") + r % 100;
								break;
							case "h":
								var h = t.getHours();
								d += c("h", h > 12 ? h - 12 : 0 === h ? 12 : h, 2);
								break;
							case "H":
								d += c("H", t.getHours(), 2);
								break;
							case "i":
								d += c("i", t.getMinutes(), 2);
								break;
							case "s":
								d += c("s", t.getSeconds(), 2);
								break;
							case "a":
								d += t.getHours() > 11 ? o.pmText : o.amText;
								break;
							case "A":
								d += t.getHours() > 11 ? o.pmText.toUpperCase() : o.amText
							.toUpperCase();
								break;
							case "'":
								l("'") ? d += "'" : u = !0;
								break;
							default:
								d += e.charAt(i)
						}
					return d
				},
				parseDate: function(e, t, n) {
					var i = s.extend({}, a.util.datetime.defaults, n),
						r = i.defaultValue && i.defaultValue.getTime ? i.defaultValue : new Date;
					if (!e || !t) return r;
					if (t.getTime) return t;
					t = "object" == typeof t ? t.toString() : t + "";
					var o, l = i.shortYearCutoff,
						c = i.getYear(r),
						m = i.getMonth(r) + 1,
						d = i.getDay(r),
						u = -1,
						h = r.getHours(),
						f = r.getMinutes(),
						p = 0,
						b = -1,
						v = !1,
						g = function(t) {
							var a = o + 1 < e.length && e.charAt(o + 1) == t;
							return a && o++, a
						},
						x = function(e) {
							g(e);
							var a = "@" == e ? 14 : "!" == e ? 20 : "y" == e ? 4 : "o" == e ? 3 : 2,
								s = new RegExp("^\\d{1," + a + "}"),
								n = t.substr(w).match(s);
							return n ? (w += n[0].length, parseInt(n[0], 10)) : 0
						},
						T = function(e, a, s) {
							var n, i = g(e) ? s : a;
							for (n = 0; n < i.length; n++)
								if (t.substr(w, i[n].length).toLowerCase() == i[n].toLowerCase())
								return w += i[n].length, n + 1;
							return 0
						},
						y = function() {
							w++
						},
						w = 0;
					for (o = 0; o < e.length; o++)
						if (v) "'" != e.charAt(o) || g("'") ? y() : v = !1;
						else switch (e.charAt(o)) {
							case "d":
								d = x("d");
								break;
							case "D":
								T("D", i.dayNamesShort, i.dayNames);
								break;
							case "o":
								u = x("o");
								break;
							case "m":
								m = x("m");
								break;
							case "M":
								m = T("M", i.monthNamesShort, i.monthNames);
								break;
							case "y":
								c = x("y");
								break;
							case "H":
								h = x("H");
								break;
							case "h":
								h = x("h");
								break;
							case "i":
								f = x("i");
								break;
							case "s":
								p = x("s");
								break;
							case "a":
								b = T("a", [i.amText, i.pmText], [i.amText, i.pmText]) - 1;
								break;
							case "A":
								b = T("A", [i.amText, i.pmText], [i.amText, i.pmText]) - 1;
								break;
							case "'":
								g("'") ? y() : v = !0;
								break;
							default:
								y()
						}
					if (100 > c && (c += (new Date).getFullYear() - (new Date).getFullYear() % 100 + (c <= (
							"string" != typeof l ? l : (new Date).getFullYear() % 100 + parseInt(l,
								10)) ? 0 : -100)), u > -1)
						for (m = 1, d = u;;) {
							var C = 32 - new Date(c, m - 1, 32, 12).getDate();
							if (C >= d) break;
							m++, d -= C
						}
					h = -1 == b ? h : b && 12 > h ? h + 12 : b || 12 != h ? h : 0;
					var M = i.getDate(c, m - 1, d, h, f, p);
					return i.getYear(M) != c || i.getMonth(M) + 1 != m || i.getDay(M) != d ? r : M
				}
			}
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = a.util.datetime,
				i = n.adjustedDate,
				r = new Date,
				o = {
					startYear: r.getFullYear() - 100,
					endYear: r.getFullYear() + 1,
					separator: " ",
					dateFormat: "mm/dd/yy",
					dateDisplay: "MMddyy",
					timeFormat: "h:ii A",
					dayText: "Day",
					monthText: "Month",
					yearText: "Year",
					hourText: "Hours",
					minuteText: "Minutes",
					ampmText: "&nbsp;",
					secText: "Seconds",
					nowText: "Now",
					todayText: "Today"
				},
				l = function(t) {
					function r(e, t, a, s) {
						return Math.min(s, Math.floor(e / t) * t + a)
					}

					function l(e) {
						return 10 > e ? "0" + e : e
					}

					function c(e) {
						var t, a, s, n = [];
						if (e) {
							for (t = 0; t < e.length; t++)
								if (a = e[t], a.start && a.start.getTime)
									for (s = new Date(a.start); s <= a.end;) n.push(i(s.getFullYear(), s.getMonth(),
										s.getDate())), s.setDate(s.getDate() + 1);
								else n.push(a);
							return n
						}
						return e
					}

					function m(e, t, a) {
						return Math.floor((a - t) / e) * e + t
					}

					function d(e) {
						return {
							value: e,
							display: (/yy/i.test(Z) ? e : (e + "").substr(2, 2)) + (J.yearSuffix || "")
						}
					}

					function u(e) {
						return e
					}

					function h(e) {
						return J.getYear(e)
					}

					function f(e) {
						return J.getMonth(e)
					}

					function p(e) {
						return J.getDay(e)
					}

					function b(e) {
						var t = e.getHours();
						return t = nt && t >= 12 ? t - 12 : t, r(t, ot, dt, ft)
					}

					function v(e) {
						return r(e.getMinutes(), lt, ut, pt)
					}

					function g(e) {
						return r(e.getSeconds(), ct, ht, bt)
					}

					function x(e) {
						return e.getMilliseconds()
					}

					function T(e) {
						return e.getHours() > 11 ? 1 : 0
					}

					function y(e) {
						return e.getFullYear() + "-" + l(e.getMonth() + 1) + "-" + l(e.getDate())
					}

					function w(e) {
						return r(Math.round((e.getTime() - new Date(e).setHours(0, 0, 0, 0)) / 1e3), O, 0, 86400)
					}

					function C(t, a, s, n) {
						var i;
						return Y[a] === e || (i = +t[Y[a]], isNaN(i)) ? s ? yt[a](s) : z[a] !== e ? z[a] : yt[a](
							n) : i
					}

					function M(t) {
						var a, s = new Date((new Date).setHours(0, 0, 0, 0));
						if (null === t) return t;
						Y.dd !== e && (a = t[Y.dd].split("-"), a = new Date(a[0], a[1] - 1, a[2])), Y.tt !== e && (
							a = a || s, a = new Date(a.getTime() + t[Y.tt] % 86400 * 1e3));
						var n = C(t, "y", a, s),
							i = C(t, "m", a, s),
							r = Math.min(C(t, "d", a, s), J.getMaxDayOfMonth(n, i)),
							o = C(t, "h", a, s);
						return J.getDate(n, i, r, nt && C(t, "a", a, s) ? o + 12 : o, C(t, "i", a, s), C(t, "s", a,
							s), C(t, "u", a, s))
					}

					function S(t, a) {
						var s, n, i = ["y", "m", "d", "a", "h", "i", "s", "u", "dd", "tt"],
							r = [];
						if (null === t || t === e) return t;
						for (s = 0; s < i.length; s++) n = i[s], Y[n] !== e && (r[Y[n]] = yt[n](t)), a && (z[s] =
							yt[n](t));
						return r
					}

					function D(e, t) {
						return t ? Math.floor(new Date(e) / 864e5) : e.getMonth() + 12 * (e.getFullYear() - 1970)
					}

					function k(e) {
						var t = /d/i.test(e);
						return {
							label: "",
							cssClass: "mbsc-dt-whl-date",
							min: D(y(tt), t),
							max: D(y(at), t),
							data: function(a) {
								var s = new Date((new Date).setHours(0, 0, 0, 0)),
									i = t ? new Date(864e5 * a) : new Date(1970, a, 1);
								return t && (i = new Date(i.getUTCFullYear(), i.getUTCMonth(), i
							.getUTCDate())), {
									invalid: t && !F(i, !0),
									value: y(i),
									display: s.getTime() == i.getTime() ? J.todayText : n.formatDate(e, i,
										J)
								}
							},
							getIndex: function(e) {
								return D(e, t)
							}
						}
					}

					function _(e) {
						var t, a, s, i = [];
						for (/s/i.test(e) ? a = ct : /i/i.test(e) ? a = 60 * lt : /h/i.test(e) && (a = 3600 * ot),
							O = xt.tt = a, t = 0; 86400 > t; t += a) s = new Date((new Date).setHours(0, 0, 0, 0) +
							1e3 * t), i.push({
							value: t,
							display: n.formatDate(e, s, J)
						});
						return {
							label: "",
							cssClass: "mbsc-dt-whl-time",
							data: i
						}
					}

					function A() {
						var t, a, s, n, i, r, o, c, m = 0,
							h = [],
							f = [],
							p = [];
						if (q.match(/date/i)) {
							for (t = G.split(/\|/.test(G) ? "|" : ""), n = 0; n < t.length; n++)
								if (s = t[n], r = 0, s.length)
									if (/y/i.test(s) && r++, /m/i.test(s) && r++, /d/i.test(s) && r++, r > 1 && Y
										.dd === e) Y.dd = m, m++, f.push(k(s)), p = f, P = !0;
									else if (/y/i.test(s) && Y.y === e) Y.y = m, m++, f.push({
								cssClass: "mbsc-dt-whl-y",
								label: J.yearText,
								min: J.getYear(tt),
								max: J.getYear(at),
								data: d,
								getIndex: u
							});
							else if (/m/i.test(s) && Y.m === e) {
								for (Y.m = m, o = [], m++, i = 0; 12 > i; i++) c = Z.replace(/[dy]/gi, "").replace(
									/mm/, l(i + 1) + (J.monthSuffix || "")).replace(/m/, i + 1 + (J
									.monthSuffix || "")), o.push({
									value: i,
									display: /MM/.test(c) ? c.replace(/MM/, '<span class="mbsc-dt-month">' +
										J.monthNames[i] + "</span>") : c.replace(/M/,
										'<span class="mbsc-dt-month">' + J.monthNamesShort[i] +
										"</span>")
								});
								f.push({
									cssClass: "mbsc-dt-whl-m",
									label: J.monthText,
									data: o
								})
							} else if (/d/i.test(s) && Y.d === e) {
								for (Y.d = m, o = [], m++, i = 1; 32 > i; i++) o.push({
									value: i,
									display: (/dd/i.test(Z) ? l(i) : i) + (J.daySuffix || "")
								});
								f.push({
									cssClass: "mbsc-dt-whl-d",
									label: J.dayText,
									data: o
								})
							}
							h.push(f)
						}
						if (q.match(/time/i)) {
							for (a = X.split(/\|/.test(X) ? "|" : ""), n = 0; n < a.length; n++)
								if (s = a[n], r = 0, s.length && (/h/i.test(s) && r++, /i/i.test(s) && r++, /s/i
										.test(s) && r++, /a/i.test(s) && r++), r > 1 && Y.tt === e) Y.tt = m, m++, p
									.push(_(s));
								else if (/h/i.test(s) && Y.h === e) {
								for (o = [], Y.h = m, m++, i = dt;
									(nt ? 12 : 24) > i; i += ot) o.push({
									value: i,
									display: nt && 0 === i ? 12 : /hh/i.test(Q) ? l(i) : i
								});
								p.push({
									cssClass: "mbsc-dt-whl-h",
									label: J.hourText,
									data: o
								})
							} else if (/i/i.test(s) && Y.i === e) {
								for (o = [], Y.i = m, m++, i = ut; 60 > i; i += lt) o.push({
									value: i,
									display: /ii/i.test(Q) ? l(i) : i
								});
								p.push({
									cssClass: "mbsc-dt-whl-i",
									label: J.minuteText,
									data: o
								})
							} else if (/s/i.test(s) && Y.s === e) {
								for (o = [], Y.s = m, m++, i = ht; 60 > i; i += ct) o.push({
									value: i,
									display: /ss/i.test(Q) ? l(i) : i
								});
								p.push({
									cssClass: "mbsc-dt-whl-s",
									label: J.secText,
									data: o
								})
							} else /a/i.test(s) && Y.a === e && (Y.a = m, m++, p.push({
								cssClass: "mbsc-dt-whl-a",
								label: J.ampmText,
								data: /A/.test(s) ? [{
									value: 0,
									display: J.amText.toUpperCase()
								}, {
									value: 1,
									display: J.pmText.toUpperCase()
								}] : [{
									value: 0,
									display: J.amText
								}, {
									value: 1,
									display: J.pmText
								}]
							}));
							p != f && h.push(p)
						}
						return h
					}

					function V(e) {
						var t, a, s, i = {};
						if (e.is("input")) {
							switch (e.attr("type")) {
								case "date":
									t = "yy-mm-dd";
									break;
								case "datetime":
									t = "yy-mm-ddTHH:ii:ssZ";
									break;
								case "datetime-local":
									t = "yy-mm-ddTHH:ii:ss";
									break;
								case "month":
									t = "yy-mm", i.dateOrder = "mmyy";
									break;
								case "time":
									t = "HH:ii:ss"
							}
							i.format = t, a = e.attr("min"), s = e.attr("max"), a && (i.min = n.parseDate(t, a)),
								s && (i.max = n.parseDate(t, s))
						}
						return i
					}

					function N(e, t) {
						var a, s, n = !1,
							i = !1,
							r = 0,
							o = 0;
						if (tt = M(S(tt)), at = M(S(at)), F(e)) return e;
						if (tt > e && (e = tt), e > at && (e = at), a = e, s = e, 2 !== t)
							for (n = F(a); !n && at > a;) a = new Date(a.getTime() + 864e5), n = F(a), r++;
						if (1 !== t)
							for (i = F(s); !i && s > tt;) s = new Date(s.getTime() - 864e5), i = F(s), o++;
						return 1 === t && n ? a : 2 === t && i ? s : r >= o && i ? s : a
					}

					function F(e, t) {
						return !t && tt > e ? !1 : !t && e > at ? !1 : L(e, B) ? !0 : L(e, R) ? !1 : !0
					}

					function L(e, t) {
						var a, s, n;
						if (t)
							for (s = 0; s < t.length; s++)
								if (a = t[s], n = a + "", !a.start)
									if (a.getTime) {
										if (e.getFullYear() == a.getFullYear() && e.getMonth() == a.getMonth() && e
											.getDate() == a.getDate()) return !0
									} else if (n.match(/w/i)) {
							if (n = +n.replace("w", ""), n == e.getDay()) return !0
						} else if (n = n.split("/"), n[1]) {
							if (n[0] - 1 == e.getMonth() && n[1] == e.getDate()) return !0
						} else if (n[0] == e.getDate()) return !0;
						return !1
					}

					function I(e, t, a, s, n, i, r) {
						var o, l, c, m;
						if (e)
							for (l = 0; l < e.length; l++)
								if (o = e[l], m = o + "", !o.start)
									if (o.getTime) J.getYear(o) == t && J.getMonth(o) == a && (i[J.getDay(o)] = r);
									else if (m.match(/w/i))
							for (m = +m.replace("w", ""), c = m - s; n > c; c += 7) c >= 0 && (i[c + 1] = r);
						else m = m.split("/"), m[1] ? m[0] - 1 == a && (i[m[1]] = r) : i[m[0]] = r
					}

					function H(t, a, s, n, i, o, l, c) {
						var m, d, u, h, f, p, b, v, g, x, T, y, w, C, M, S, D, k, _, A, V = {},
							N = J.getDate(n, i, o),
							F = ["a", "h", "i", "s"];
						if (t) {
							for (b = 0; b < t.length; b++) T = t[b], T.start && (T.apply = !1, u = T.d, D = u + "",
								k = D.split("/"), u && (u.getTime && n == J.getYear(u) && i == J.getMonth(u) &&
									o == J.getDay(u) || !D.match(/w/i) && (k[1] && o == k[1] && i == k[0] - 1 ||
										!k[1] && o == k[0]) || D.match(/w/i) && N.getDay() == +D.replace("w",
										"")) && (T.apply = !0, V[N] = !0));
							for (b = 0; b < t.length; b++)
								if (T = t[b], m = 0, S = 0, v = vt[s], g = gt[s], C = !0, M = !0, d = !1, T.start &&
									(T.apply || !T.d && !V[N])) {
									for (y = T.start.split(":"), w = T.end.split(":"), x = 0; 3 > x; x++) y[x] ===
										e && (y[x] = 0), w[x] === e && (w[x] = 59), y[x] = +y[x], w[x] = +w[x];
									if ("tt" == s) v = r(Math.round((new Date(N).setHours(y[0], y[1], y[2]) -
										new Date(N).setHours(0, 0, 0, 0)) / 1e3), O, 0, 86400), g = r(Math
										.round((new Date(N).setHours(w[0], w[1], w[2]) - new Date(N).setHours(0,
											0, 0, 0)) / 1e3), O, 0, 86400);
									else {
										for (y.unshift(y[0] > 11 ? 1 : 0), w.unshift(w[0] > 11 ? 1 : 0), nt && (y[
												1] >= 12 && (y[1] = y[1] - 12), w[1] >= 12 && (w[1] = w[1] - 12)),
											x = 0; a > x; x++) j[x] !== e && (_ = r(y[x], xt[F[x]], vt[F[x]], gt[F[
												x]]), A = r(w[x], xt[F[x]], vt[F[x]], gt[F[x]]), h = 0, f = 0,
											p = 0, nt && 1 == x && (h = y[0] ? 12 : 0, f = w[0] ? 12 : 0, p = j[
												0] ? 12 : 0), C || (_ = 0), M || (A = gt[F[x]]), (C || M) && _ +
											h < j[x] + p && j[x] + p < A + f && (d = !0), j[x] != _ && (C = !1),
											j[x] != A && (M = !1));
										if (!c)
											for (x = a + 1; 4 > x; x++) y[x] > 0 && (m = xt[s]), w[x] < gt[F[x]] &&
												(S = xt[s]);
										d || (_ = r(y[a], xt[s], vt[s], gt[s]) + m, A = r(w[a], xt[s], vt[s], gt[
											s]) - S, C && (v = _), M && (g = A))
									}
									if (C || M || d)
										for (x = v; g >= x; x += xt[s]) l[x] = !c
								}
						}
					}
					var O, P, E, Y = {},
						z = {},
						j = [],
						W = V(s(this)),
						$ = s.extend({}, t.settings),
						J = s.extend(t.settings, a.util.datetime.defaults, o, W, $),
						R = c(J.invalid),
						B = c(J.valid),
						q = J.preset,
						U = "datetime" == q ? J.dateFormat + J.separator + J.timeFormat : "time" == q ? J
						.timeFormat : J.dateFormat,
						K = W.format || U,
						G = J.dateWheels || J.dateFormat,
						X = J.timeWheels || J.timeFormat,
						Z = J.dateWheels || J.dateDisplay,
						Q = X,
						et = J.baseTheme || J.theme,
						tt = J.min || i(J.startYear, 0, 1),
						at = J.max || i(J.endYear, 11, 31, 23, 59, 59),
						st = /time/i.test(q),
						nt = /h/.test(Q),
						it = /D/.test(Z),
						rt = J.steps || {},
						ot = rt.hour || J.stepHour || 1,
						lt = rt.minute || J.stepMinute || 1,
						ct = rt.second || J.stepSecond || 1,
						mt = rt.zeroBased,
						dt = mt ? 0 : tt.getHours() % ot,
						ut = mt ? 0 : tt.getMinutes() % lt,
						ht = mt ? 0 : tt.getSeconds() % ct,
						ft = m(ot, dt, nt ? 11 : 23),
						pt = m(lt, ut, 59),
						bt = m(lt, ut, 59),
						vt = {
							y: tt.getFullYear(),
							m: 0,
							d: 1,
							h: dt,
							i: ut,
							s: ht,
							a: 0,
							tt: 0
						},
						gt = {
							y: at.getFullYear(),
							m: 11,
							d: 31,
							h: ft,
							i: pt,
							s: bt,
							a: 1,
							tt: 86400
						},
						xt = {
							y: 1,
							m: 1,
							d: 1,
							h: ot,
							i: lt,
							s: ct,
							a: 1,
							tt: 1
						},
						Tt = {
							"android-holo": 40,
							bootstrap: 46,
							ios: 50,
							jqm: 46,
							material: 46,
							mobiscroll: 46,
							wp: 50
						},
						yt = {
							y: h,
							m: f,
							d: p,
							h: b,
							i: v,
							s: g,
							u: x,
							a: T,
							dd: y,
							tt: w
						};
					return t.getDate = t.getVal = function(e) {
						return t._hasValue || e ? M(t.getArrayVal(e)) : null
					}, t.setDate = function(e, a, s, n, i) {
						t.setArrayVal(S(e), a, i, n, s)
					}, E = A(), t.format = U, t.order = Y, t.handlers.now = function() {
						t.setDate(new Date, t.live, 1e3, !0, !0)
					}, t.buttons.now = {
						text: J.nowText,
						handler: "now"
					}, {
						minWidth: P && st ? Tt[et] : e,
						compClass: "mbsc-dt",
						wheels: E,
						headerText: J.headerText ? function() {
							return n.formatDate(U, M(t.getArrayVal(!0)), J)
						} : !1,
						formatValue: function(e) {
							return n.formatDate(K, M(e), J)
						},
						parseValue: function(e) {
							return e || (z = {}), S(e ? n.parseDate(K, e, J) : J.defaultValue && J
								.defaultValue.getTime ? J.defaultValue : new Date, !!e && !!e.getTime)
						},
						validate: function(a) {
							var n, i, r, o, l = a.values,
								c = a.index,
								m = a.direction,
								d = t.settings.wheels[0][Y.d],
								u = N(M(l), m),
								h = S(u),
								f = [],
								p = {},
								b = yt.y(u),
								v = yt.m(u),
								g = J.getMaxDayOfMonth(b, v),
								x = !0,
								T = !0;
							if (s.each(["dd", "y", "m", "d", "tt", "a", "h", "i", "s"], function(t, a) {
									if (Y[a] !== e) {
										var n = vt[a],
											r = gt[a],
											o = yt[a](u);
										if (f[Y[a]] = [], x && tt && (n = yt[a](tt)), T && at && (r =
												yt[a](at)), "y" != a && "dd" != a)
											for (i = vt[a]; i <= gt[a]; i += xt[a])(n > i || i > r) &&
												f[Y[a]].push(i);
										if (n > o && (o = n), o > r && (o = r), x && (x = o == n), T &&
											(T = o == r), "d" == a) {
											var l = J.getDate(b, v, 1).getDay(),
												c = {};
											I(R, b, v, l, g, c, 1), I(B, b, v, l, g, c, 0), s.each(c,
												function(e, t) {
													t && f[Y[a]].push(e)
												})
										}
									}
								}), st && s.each(["a", "h", "i", "s", "tt"], function(a, n) {
									var i = yt[n](u),
										r = yt.d(u),
										o = {};
									Y[n] !== e && (H(R, a, n, b, v, r, o, 0), H(B, a, n, b, v, r, o, 1),
										s.each(o, function(e, t) {
											t && f[Y[n]].push(e)
										}), j[a] = t.getValidValue(Y[n], i, m, o))
								}), d && (d._length !== g || it && (c === e || c === Y.y || c === Y.m))) {
								for (p[Y.d] = d, d.data = [], n = 1; g >= n; n++) o = J.getDate(b, v, n)
									.getDay(), r = Z.replace(/[my]/gi, "").replace(/dd/, (10 > n ? "0" + n :
										n) + (J.daySuffix || "")).replace(/d/, n + (J.daySuffix || "")), d
									.data.push({
										value: n,
										display: r.match(/DD/) ? r.replace(/DD/,
											'<span class="mbsc-dt-day">' + J.dayNames[o] + "</span>"
											) : r.replace(/D/, '<span class="mbsc-dt-day">' + J
											.dayNamesShort[o] + "</span>")
									});
								t._tempWheelArray[Y.d] = h[Y.d], t.changeWheel(p)
							}
							return {
								disabled: f,
								valid: h
							}
						}
					}
				};
			s.each(["date", "time", "datetime"], function(e, t) {
				a.presets.scroller[t] = l
			})
		}(),
		function() {
			t.$.each(["date", "time", "datetime"], function(e, a) {
				t.presetShort(a)
			})
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = {
					invalid: [],
					showInput: !0,
					inputClass: ""
				};
			a.presets.scroller.list = function(t) {
				function a(e, t, a) {
					for (var s = 0, n = []; e > s;) n[s] = i(a, s, t), s++;
					return n
				}

				function i(e, t, a) {
					for (var s, n = 0, i = a, r = []; t > n;) {
						var o = e[n];
						for (s in i)
							if (i[s].key == o) {
								i = i[s].children;
								break
							} n++
					}
					for (n = 0; n < i.length;) i[n].invalid && r.push(i[n].key), n++;
					return r
				}

				function r(e, t) {
					for (var a = []; e;) a[--e] = !0;
					return a[t] = !1, a
				}

				function o(e) {
					var t, a = [];
					for (t = 0; e > t; t++) a[t] = T.labels && T.labels[t] ? T.labels[t] : t;
					return a
				}

				function l(t, a, s) {
					var n, i, r, o = 0,
						l = [
							[]
						],
						d = A;
					if (a)
						for (n = 0; a > n; n++) w ? l[0][n] = {} : l[n] = [{}];
					for (; o < t.length;) {
						for (w ? l[0][o] = m(d, V[o]) : l[o] = [m(d, V[o])], n = 0, r = e; n < d.length && r ===
							e;) d[n].key == t[o] && (s !== e && s >= o || s === e) && (r = n), n++;
						if (r !== e && d[r].children) o++, d = d[r].children;
						else {
							if (!(i = c(d)) || !i.children) return l;
							o++, d = i.children
						}
					}
					return l
				}

				function c(e, t) {
					if (!e) return !1;
					for (var a, s = 0; s < e.length;)
						if (!(a = e[s++]).invalid) return t ? s - 1 : a;
					return !1
				}

				function m(e, t) {
					for (var a = {
							data: [],
							label: t
						}, s = 0; s < e.length;) a.data.push({
						value: e[s].key,
						display: e[s].value
					}), s++;
					return a
				}

				function d(e) {
					t._isVisible && s(".mbsc-sc-whl-w", t._markup).css("display", "").slice(e).hide()
				}

				function u(e) {
					for (var t, a = [], s = e, n = !0, i = 0; n;) t = c(s), a[i++] = t.key, n = t.children, n &&
						(s = n);
					return a
				}

				function h(t, a) {
					var s, n, i, r = [],
						o = A,
						l = 0,
						m = !1;
					if (t[l] !== e && a >= l)
						for (s = 0, n = t[l], i = e; s < o.length && i === e;) o[s].key != t[l] || o[s]
							.invalid || (i = s), s++;
					else i = c(o, !0), n = o[i].key;
					for (m = i !== e ? o[i].children : !1, r[l] = n; m;) {
						if (o = o[i].children, l++, m = !1, i = e, t[l] !== e && a >= l)
							for (s = 0, n = t[l], i = e; s < o.length && i === e;) o[s].key != t[l] || o[s]
								.invalid || (i = s), s++;
						else i = c(o, !0), i = i === !1 ? e : i, n = o[i].key;
						m = i !== e && c(o[i].children) ? o[i].children : !1, r[l] = n
					}
					return {
						lvl: l + 1,
						nVector: r
					}
				}

				function f(a) {
					var n = [];
					return D = D > k++ ? D : k, a.children("li").each(function(a) {
						var i = s(this),
							r = i.clone();
						r.children("ul,ol").remove();
						var o = t._processMarkup ? t._processMarkup(r) : r.html().replace(/^\s\s*/, "")
							.replace(/\s\s*$/, ""),
							l = i.attr("data-invalid") ? !0 : !1,
							c = {
								key: i.attr("data-val") === e || null === i.attr("data-val") ? a : i
									.attr("data-val"),
								value: o,
								invalid: l,
								children: null
							},
							m = i.children("ul,ol");
						m.length && (c.children = f(m)), n.push(c)
					}), k--, n
				}

				function p(e, a, s) {
					var n, i = (a || 0) + 1,
						r = [],
						o = {},
						c = {};
					for (o = l(e, null, a), n = 0; n < e.length; n++) t._tempWheelArray[n] = e[n] = s.nVector[
						n] || 0;
					for (; i < s.lvl;) c[i] = w ? o[0][i] : o[i][0], r.push(i++);
					d(s.lvl), _ = e.slice(0), r.length && (v = !0, t.changeWheel(c))
				}
				var b, v, g, x = s.extend({}, t.settings),
					T = s.extend(t.settings, n, x),
					y = T.layout || (/top|bottom/.test(T.display) ? "liquid" : ""),
					w = "liquid" == y,
					C = T.readonly,
					M = s(this),
					S = this.id + "_dummy",
					D = 0,
					k = 0,
					_ = [],
					A = T.wheelArray || f(M),
					V = o(D),
					N = u(A),
					F = l(N, D);
				return s("#" + S).remove(), T.showInput && (b = s('<input type="text" id="' + S +
						'" value="" class="' + T.inputClass + '" placeholder="' + (T.placeholder || "") +
						'" readonly />').insertBefore(M), T.anchor = b, t.attachShow(b)), T.wheelArray || M
					.hide(), {
						wheels: F,
						layout: y,
						headerText: !1,
						setOnTap: 1 == D,
						formatValue: function(t) {
							return g === e && (g = h(t, t.length).lvl), t.slice(0, g).join(" ")
						},
						parseValue: function(e) {
							return e ? (e + "").split(" ") : (T.defaultValue || N).slice(0)
						},
						onBeforeShow: function() {
							var e = t.getArrayVal(!0);
							_ = e.slice(0), T.wheels = l(e, D, D), v = !0
						},
						onWheelGestureStart: function(e) {
							T.readonly = r(D, e.index)
						},
						onWheelAnimationEnd: function(e) {
							var a = e.index,
								s = t.getArrayVal(!0),
								n = h(s, a);
							g = n.lvl, T.readonly = C, s[a] != _[a] && p(s, a, n)
						},
						onFill: function(t) {
							g = e, b && b.val(t.valueText)
						},
						validate: function(t) {
							var s = t.values,
								n = t.index,
								i = h(s, s.length);
							return g = i.lvl, n === e && (d(i.lvl), v || p(s, n, i)), v = !1, {
								disabled: a(g, A, s)
							}
						},
						onDestroy: function() {
							b && b.remove(), M.show()
						}
					}
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.scroller;
			e.presetShort("image"), s.image = function(e) {
				return e.settings.enhance && (e._processMarkup = function(e) {
					var t = e.attr("data-icon");
					return e.children().each(function(e, t) {
						t = a(t), t.is("img") ? a('<div class="mbsc-img-c"></div>').insertAfter(
							t).append(t.addClass("mbsc-img")) : t.is("p") && t.addClass(
							"mbsc-img-txt")
					}), t && e.prepend('<div class="mbsc-ic mbsc-ic-' + t + '"></div'), e.html(
						'<div class="mbsc-img-w">' + e.html() + "</div>"), e.html()
				}), s.list.call(this, e)
			}
		}(),
		function() {
			var e = t,
				a = e.presets.scroller;
			a.treelist = a.list, e.presetShort("list"), e.presetShort("treelist")
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = a.util,
				i = n.isString,
				r = {
					inputClass: "",
					invalid: [],
					rtl: !1,
					showInput: !0,
					groupLabel: "Groups",
					checkIcon: "checkmark",
					dataText: "text",
					dataValue: "value",
					dataGroup: "group",
					dataDisabled: "disabled"
				};
			a.presetShort("select"), a.presets.scroller.select = function(t) {
				function a() {
					var t, a, n, i, r, o = 0,
						l = 0,
						c = {};
					M = {}, T = {}, C = [], x = [], R.length = 0, E ? s.each(V.data, function(s, o) {
						i = o[V.dataText], r = o[V.dataValue], a = o[V.dataGroup], n = {
								value: r,
								text: i,
								index: s
							}, M[r] = n, C.push(n), Y && (c[a] === e ? (t = {
								text: a,
								value: l,
								options: [],
								index: l
							}, T[l] = t, c[a] = l, x.push(t), l++) : t = T[c[a]], W && (n.index = t
								.options.length), n.group = c[a], t.options.push(n)), o[V
							.dataDisabled] && R.push(r)
					}) : Y ? s("optgroup", _).each(function(e) {
						T[e] = {
							text: this.label,
							value: e,
							options: [],
							index: e
						}, x.push(T[e]), s("option", this).each(function(t) {
							n = {
									value: this.value,
									text: this.text,
									index: W ? t : o++,
									group: e
								}, M[this.value] = n, C.push(n), T[e].options.push(n), this
								.disabled && R.push(this.value)
						})
					}) : s("option", _).each(function(e) {
						n = {
							value: this.value,
							text: this.text,
							index: e
						}, M[this.value] = n, C.push(n), this.disabled && R.push(this.value)
					}), C.length && (v = C[0].value), $ && (C = [], o = 0, s.each(T, function(e, t) {
						r = "__group" + e, n = {
							text: t.text,
							value: r,
							group: e,
							index: o++,
							cssClass: "mbsc-sel-gr"
						}, M[r] = n, C.push(n), R.push(n.value), s.each(t.options, function(e,
							t) {
							t.index = o++, C.push(t)
						})
					}))
				}

				function o(e, t, a) {
					var s, n = [];
					for (s = 0; s < e.length; s++) n.push({
						value: e[s].value,
						display: e[s].text,
						cssClass: e[s].cssClass
					});
					return {
						circular: !1,
						multiple: t,
						data: n,
						label: a
					}
				}

				function l() {
					return o(x, !1, V.groupLabel)
				}

				function c() {
					return o(W ? T[g].options : C, I, P)
				}

				function m() {
					var e, t, a = [
						[]
					];
					return j && (e = l(), L ? a[0][y] = e : a[y] = [e]), t = c(), L ? a[0][S] = t : a[S] = [t],
						a
				}

				function d(t) {
					I && (t && i(t) && (t = t.split(",")), s.isArray(t) && (t = t[0])), w = t !== e && null !==
						t && "" !== t && M[t] ? t : v, j && (g = M[w] ? M[w].group : null)
				}

				function u(e, a) {
					var s = e ? t._tempWheelArray : t._hasValue ? t._wheelArray : null;
					return s ? V.group && a ? s : s[S] : null
				}

				function h(e) {
					var a, s, n = [];
					if (I) {
						for (a in t._tempSelected[S]) n.push(M[a] ? M[a].text : "");
						return n.join(", ")
					}
					return s = e[S], M[s] ? M[s].text : ""
				}

				function f() {
					var e = t.getVal(),
						a = t._tempValue;
					b.val(a), _.val(e)
				}

				function p() {
					var e = {};
					e[S] = c(), D = !0, t.changeWheel(e)
				}
				var b, v, g, x, T, y, w, C, M, S, D, k = 1e3,
					_ = s(this),
					A = s.extend({}, t.settings),
					V = s.extend(t.settings, r, A),
					N = V.readonly,
					F = V.layout || (/top|bottom/.test(V.display) ? "liquid" : ""),
					L = "liquid" == F,
					I = n.isNumeric(V.select) ? V.select : "multiple" == V.select || _.prop("multiple"),
					H = this.id + "_dummy",
					O = s('label[for="' + this.id + '"]').attr("for", H),
					P = V.label !== e ? V.label : O.length ? O.text() : _.attr("name"),
					E = !!V.data,
					Y = E ? !!V.group : s("optgroup", _).length,
					z = V.group,
					j = Y && z && z.groupWheel !== !1,
					W = Y && z && j && z.clustered === !0,
					$ = Y && (!z || z.header !== !1 && !W),
					J = _.val() || [],
					R = [];
				return t.setVal = function(e, a, s, r, o) {
						I && (e && i(e) && (e = e.split(",")), t._tempSelected[S] = n.arrayToObject(e), r || (t
							._selected[S] = n.arrayToObject(e)), e = e ? e[0] : null), t._setVal(e, a, s, r,
							o)
					}, t.getVal = function(e, a) {
						return I ? n.objectToArray(e ? t._tempSelected[S] : t._selected[S]) : u(e, a)
					}, t.refresh = function() {
						var e = {};
						a(), V.wheels = m(), d(w), e[S] = c(), t._tempWheelArray[S] = w, j && (e[y] = l(), t
							._tempWheelArray[y] = g), t._isVisible && t.changeWheel(e, 0, !0)
					}, V.invalid.length || (V.invalid = R), j ? (y = 0, S = 1) : (y = -1, S = 0), I && (_.prop(
						"multiple", !0), t._selected[S] = {}, J && i(J) && (J = J.split(",")), t._selected[
						S] = n.arrayToObject(J)), s("#" + H).remove(), _.next().is("input.mbsc-control") ? b = _
					.off(".mbsc-form").next().removeAttr("tabindex") : (b = s('<input type="text" id="' + H +
							'" class="mbsc-control mbsc-control-ev ' + V.inputClass + '" readonly />'), V
						.showInput && b.insertBefore(_)), t.attachShow(b.attr("placeholder", V.placeholder ||
						"")), _.addClass("mbsc-sel-hdn").attr("tabindex", -1), a(), d(_.val()), {
						layout: F,
						headerText: !1,
						anchor: b,
						compClass: "mbsc-sel" + (j ? " mbsc-sel-gr-whl" : "") + (I ? " mbsc-sel-multi" : ""),
						setOnTap: j ? [!1, !0] : !0,
						formatValue: h,
						parseValue: function(t) {
							return d(t === e ? _.val() : t), j ? [g, w] : [w]
						},
						validate: function(t) {
							var a = t.index,
								s = [];
							return s[S] = V.invalid, W && !D && a === e && p(), D = !1, {
								disabled: s
							}
						},
						onRead: f,
						onFill: f,
						onBeforeShow: function() {
							I && V.counter && (V.headerText = function() {
								var e = 0;
								return s.each(t._tempSelected[S], function() {
									e++
								}), (e > 1 ? V.selectedPluralText || V.selectedText : V
									.selectedText).replace(/{count}/, e)
							}), d(_.val()), t.settings.wheels = m(), D = !0
						},
						onWheelGestureStart: function(e) {
							e.index == y && (V.readonly = [!1, !0])
						},
						onWheelAnimationEnd: function(e) {
							var a = t.getArrayVal(!0);
							e.index == y ? (V.readonly = N, a[y] != g && (g = a[y], w = T[g].options[0]
									.value, a[S] = w, W ? p() : t.setArrayVal(a, !1, !1, !0, k))) : e
								.index == S && a[S] != w && (w = a[S], j && M[w].group != g && (g = M[w]
									.group, a[y] = g, t.setArrayVal(a, !1, !1, !0, k)))
						},
						onDestroy: function() {
							b.hasClass("mbsc-control") || b.remove(), _.removeClass("mbsc-sel-hdn")
								.removeAttr("tabindex")
						}
					}
			}
		}(),
		function(e) {
			var a = function() {},
				s = t,
				n = s.$;
			s.util.addIcon = function(e, t) {
				var a = {},
					s = e.parent(),
					i = s.find(".mbsc-err-msg"),
					r = e.attr("data-icon-align") || "left",
					o = e.attr("data-icon");
				n('<span class="mbsc-input-wrap"></span>').insertAfter(e).append(e), i && s.find(
					".mbsc-input-wrap").append(i), o && (-1 !== o.indexOf("{") ? a = JSON.parse(o) : a[r] =
					o), (o || t) && (n.extend(a, t), s.addClass((a.right ? "mbsc-ic-right " : "") + (a
					.left ? " mbsc-ic-left" : "")).find(".mbsc-input-wrap").append(a.left ?
					'<span class="mbsc-input-ic mbsc-left-ic mbsc-ic mbsc-ic-' + a.left + '"></span>' :
					"").append(a.right ? '<span class="mbsc-input-ic mbsc-right-ic mbsc-ic mbsc-ic-' + a
					.right + '"></span>' : ""))
			}, s.classes.Progress = function(t, i, r) {
				function o() {
					var e = l("value", v);
					e !== y && c(e)
				}

				function l(t, a) {
					var s = d.attr(t);
					return s === e || "" === s ? a : +s
				}

				function c(t, a, s, n) {
					t = Math.min(g, Math.max(t, v)), h.css("width", 100 * (t - v) / (g - v) + "%"), s === e && (
						s = !0), n === e && (n = s), (t !== y || a) && C._display(t), t !== y && (y = t,
						s && d.attr("value", y), n && d.trigger("change"))
				}
				var m, d, u, h, f, p, b, v, g, x, T, y, w, C = this;
				s.classes.Base.call(this, t, i, !0), C._onInit = a, C._onDestroy = a, C._display = function(e) {
					w = T && x.returnAffix ? T.replace(/\{value\}/, e).replace(/\{max\}/, g) : e, f && f
						.html(w), m && m.html(w)
				}, C._attachChange = function() {
					d.on("change", o)
				}, C.init = function(a) {
					var i, r, o, c;
					if (C._init(a), x = C.settings, d = n(t), c = d.parent().hasClass("mbsc-input-wrap"),
						u = C._$parent = c ? u : d.parent(), v = C._min = a.min === e ? l("min", x.min) : a
						.min, g = C._max = a.max === e ? l("max", x.max) : a.max, y = l("value", v), i = d
						.attr("data-val") || x.val, o = d.attr("data-step-labels"), o = o ? JSON.parse(o) :
						x.stepLabels, T = d.attr("data-template") || (100 != g || x.template ? x.template :
							"{value}%"), c ? (i && (m.remove(), u.removeClass("mbsc-progress-value-" + (
								"right" == i ? "right" : "left"))), o && n(".mbsc-progress-step-label", p)
							.remove()) : (C._wrap && s.util.addIcon(d), u.find(".mbsc-input-wrap").append(
							'<span class="mbsc-progress-cont"><span class="mbsc-progress-track mbsc-progress-anim"><span class="mbsc-progress-bar"></span></span></span>'
							), h = C._$progress = u.find(".mbsc-progress-bar"), p = C._$track = u.find(
							".mbsc-progress-track")), b && u.removeClass(b), b = C._css +
						" mbsc-progress-w mbsc-control-w mbsc-" + x.theme + (x.baseTheme ? " mbsc-" + x
							.baseTheme : "") + (x.rtl ? " mbsc-rtl" : " mbsc-ltr"), u.addClass(b), d.attr(
							"min", v).attr("max", g), i && (m = n(
							'<span class="mbsc-progress-value"></span>'), u.addClass(
							"mbsc-progress-value-" + ("right" == i ? "right" : "left")).find(
							".mbsc-input-wrap").append(m)), o)
						for (r = 0; r < o.length; ++r) p.append(
							'<span class="mbsc-progress-step-label" style="' + (x.rtl ? "right" :
								"left") + ": " + 100 * (o[r] - v) / (g - v) + '%" >' + o[r] + "</span>");
					f = n(d.attr("data-target") || x.target), C._onInit(a), c || C._attachChange(), C
						.refresh(), C.trigger("onInit")
				}, C.refresh = function() {
					c(l("value", v), !0, !1)
				}, C.destroy = function(e) {
					C._onDestroy(), u.find(".mbsc-progress-cont").remove(), u.removeClass(b).find(
						".mbsc-input-wrap").before(d).remove(), d.removeClass("mbsc-control").off(
						"change", o), e || C._destroy()
				}, C.getVal = function() {
					return y
				}, C.setVal = function(e, t, a) {
					c(e, !0, t, a)
				}, r || C.init(i)
			}, s.classes.Progress.prototype = {
				_class: "progress",
				_css: "mbsc-progress",
				_hasTheme: !0,
				_hasLang: !0,
				_wrap: !0,
				_defaults: {
					min: 0,
					max: 100,
					returnAffix: !0
				}
			}, s.presetShort("progress", "Progress")
		}(),
		function(e) {
			var a = function() {},
				s = t,
				n = s.$,
				i = s.util,
				r = i.getCoord,
				o = i.testTouch;
			s.classes.Slider = function(t, l, c) {
				function m(e) {
					!o(e, this) || L || t.disabled || (X.stopProp && e.stopPropagation(), L = !0, U = !1, I = !
						1, Q = r(e, "X"), et = r(e, "Y"), E = Q, F.removeClass("mbsc-progress-anim"), S =
						K ? n(".mbsc-slider-handle", this) : k, D = S.parent().addClass("mbsc-active"),
						z = +S.attr("data-index"), st = F[0].offsetWidth, P = F[0].getBoundingClientRect()
						.left, "mousedown" === e.type && (e.preventDefault(), n(document).on("mousemove", d)
							.on("mouseup", u)))
				}

				function d(e) {
					L && (E = r(e, "X"), Y = r(e, "Y"), H = E - Q, O = Y - et, (Math.abs(H) > 5 || U) && (U = !
							0, Math.abs(rt - new Date) > 50 && (rt = new Date, T(E, X.round, R))), U ? e
						.preventDefault() : Math.abs(O) > 7 && x(e))
				}

				function u(e) {
					L && (e.preventDefault(), K || F.addClass("mbsc-progress-anim"), T(E, !0, !0), U || I || (i
						.preventClick(), it._onTap(nt[z])), x())
				}

				function h() {
					L && x()
				}

				function f() {
					var e = it._readValue(n(this)),
						t = +n(this).attr("data-index");
					e !== nt[t] && (nt[t] = e, C(e, t))
				}

				function p(e) {
					e.stopPropagation()
				}

				function b(e) {
					e.preventDefault()
				}

				function v(e) {
					var a;
					if (!t.disabled) {
						switch (e.keyCode) {
							case 38:
							case 39:
								a = 1;
								break;
							case 40:
							case 37:
								a = -1
						}
						a && (e.preventDefault(), at || (z = +n(this).attr("data-index"), C(nt[z] + G * a, z, !
							0), at = setInterval(function() {
							C(nt[z] + G * a, z, !0)
						}, 200)))
					}
				}

				function g(e) {
					e.preventDefault(), clearInterval(at), at = null
				}

				function x() {
					L = !1, D.removeClass("mbsc-active"), n(document).off("mousemove", d).off("mouseup", u)
				}

				function T(e, t, a) {
					var s = t ? Math.min(Math.round(Math.max(100 * (e - P) / st, 0) / Z / G) * G * 100 / (B -
						q), 100) : Math.max(0, Math.min(100 * (e - P) / st, 100));
					J && (s = 100 - s), C(Math.round((q + s / Z) * tt) / tt, z, a, s)
				}

				function y(e) {
					return 100 * (e - q) / (B - q)
				}

				function w(t, a) {
					var s = M.attr(t);
					return s === e || "" === s ? a : "true" === s
				}

				function C(t, a, s, n, i, r) {
					var o = k.eq(a),
						l = o.parent();
					t = Math.min(B, Math.max(t, q)), r === e && (r = s), $ ? 0 === a ? (t = Math.min(t, nt[1]),
						V.css({
							width: y(nt[1]) - y(t) + "%",
							left: J ? "auto" : y(t) + "%",
							right: J ? y(t) + "%" : "auto"
						})) : (t = Math.max(t, nt[0]), V.css({
						width: y(t) - y(nt[0]) + "%"
					})) : K || !j ? l.css({
						left: J ? "auto" : (n || y(t)) + "%",
						right: J ? (n || y(t)) + "%" : "auto"
					}) : V.css("width", (n || y(t)) + "%"), W && N.eq(a).html(t), t > q ? l.removeClass(
						"mbsc-slider-start") : (nt[a] > q || i) && l.addClass("mbsc-slider-start"), K || nt[
						a] == t && !i || it._display(t), s && nt[a] != t && (I = !0, nt[a] = t, it
						._fillValue(t, a, r)), o.attr("aria-valuenow", t)
				}
				var M, S, D, k, _, A, V, N, F, L, I, H, O, P, E, Y, z, j, W, $, J, R, B, q, U, K, G, X, Z, Q,
					et, tt, at, st, nt, it = this,
					rt = new Date;
				s.classes.Progress.call(this, t, l, !0), it._onTap = a, it.__onInit = a, it._readValue =
					function(e) {
						return +e.val()
					}, it._fillValue = function(e, t, a) {
						M.eq(t).val(e), a && M.eq(t).trigger("change")
					}, it._attachChange = function() {
						M.on(X.changeEvent, f)
					}, it._onInit = function(t) {
						var a, s, i;
						if (A && (A.removeClass("mbsc-slider-has-tooltip"), 1 != G && n(".mbsc-slider-step", F)
								.remove()), it.__onInit(), A = it._$parent, F = it._$track, V = it._$progress,
							M = A.find("input"), X = it.settings, q = it._min, B = it._max, G = t.step === e ? +
							M.attr("step") || X.step : t.step, R = w("data-live", X.live), W = w("data-tooltip",
								X.tooltip), j = w("data-highlight", X.highlight) && M.length < 3, tt = G % 1 !==
							0 ? 100 / (100 * +(G % 1).toFixed(2)) : 1, Z = 100 / (B - q) || 100, K = M.length >
							1, $ = j && 2 == M.length, J = X.rtl, nt = [], W && A.addClass(
								"mbsc-slider-has-tooltip"), 1 != G)
							for (s = (B - q) / G, a = 0; s >= a; ++a) F.append(
								'<span class="mbsc-slider-step" style="' + (J ? "right" : "left") + ":" +
								100 / s * a + '%"></span>');
						k && (i = !0, k.parent().remove()), M.each(function(e) {
								nt[e] = it._readValue(n(this)), n(this).attr("data-index", e).attr("min", q)
									.attr("max", B).attr("step", G), X.handle && (j ? V : F).append(
										'<span class="mbsc-slider-handle-cont' + ($ && !e ?
											" mbsc-slider-handle-left" : "") +
										'"><span tabindex="0" class="mbsc-slider-handle" aria-valuemin="' +
										q + '" aria-valuemax="' + B + '" data-index="' + e + '"></span>' + (
											W ? '<span class="mbsc-slider-tooltip"></span>' : "") +
										"</span>")
							}), k = A.find(".mbsc-slider-handle"), N = A.find(".mbsc-slider-tooltip"), _ = A
							.find(K ? ".mbsc-slider-handle-cont" : ".mbsc-progress-cont"), k.on("keydown", v)
							.on("keyup", g).on("blur", g), _.on("touchstart mousedown", m).on("touchmove", d)
							.on("touchend touchcancel", u).on("pointercancel", h), i || (M.on("click", p), A.on(
								"click", b))
					}, it._onDestroy = function() {
						A.off("click", b), M.off(X.changeEvent, f).off("click", p), k.off("keydown", v).off(
								"keyup", g).off("blur", g), _.off("touchstart mousedown", m).off("touchmove", d)
							.off("touchend", u).off("touchcancel pointercancel", h)
					}, it.refresh = function() {
						M.each(function(e) {
							C(it._readValue(n(this)), e, !0, !1, !0, !1)
						})
					}, it.getVal = function() {
						return K ? nt.slice(0) : nt[0]
					}, it.setVal = it._setVal = function(e, t, a) {
						n.isArray(e) || (e = [e]), n.each(e, function(e, t) {
							C(t, e, !0, !1, !0, a)
						})
					}, c || it.init(l)
			}, s.classes.Slider.prototype = {
				_class: "progress",
				_css: "mbsc-progress mbsc-slider",
				_hasTheme: !0,
				_hasLang: !0,
				_wrap: !0,
				_defaults: {
					changeEvent: "change",
					stopProp: !0,
					min: 0,
					max: 100,
					step: 1,
					live: !0,
					highlight: !0,
					handle: !0,
					round: !0,
					returnAffix: !0
				}
			}, s.presetShort("slider", "Slider")
		}(),
		function(e) {
			var a, s = function() {},
				n = t,
				i = n.$,
				r = n.util,
				o = r.getCoord,
				l = r.testTouch;
			n.classes.Form = function(e, c) {
				function m(e) {
					var t = {},
						a = e[0],
						s = e.parent(),
						n = e.attr("data-password-toggle"),
						o = e.attr("data-icon-show") || "eye",
						l = e.attr("data-icon-hide") || "eye-blocked";
					n && (t.right = "password" == a.type ? o : l), r.addIcon(e, t), n && T.tap(s.find(
						".mbsc-right-ic"), function() {
						"text" == a.type ? (a.type = "password", i(this).addClass("mbsc-ic-" + o)
							.removeClass("mbsc-ic-" + l)) : (a.type = "text", i(this).removeClass(
							"mbsc-ic-" + o).addClass("mbsc-ic-" + l))
					})
				}

				function d() {
					var e = this;
					if (!i(e).hasClass("mbsc-textarea-scroll")) {
						var t = e.scrollHeight - e.offsetHeight,
							a = e.offsetHeight + t;
						e.scrollTop = 0, e.style.height = a + "px"
					}
				}

				function u(e) {
					var t, a, s;
					e.offsetHeight && (e.style.height = "", s = e.scrollHeight - e.offsetHeight, t = e
						.offsetHeight + (s > 0 ? s : 0), a = Math.round(t / 24), a > 10 ? (e.scrollTop = t,
							t = 240 + (t - 24 * a), i(e).addClass("mbsc-textarea-scroll")) : i(e)
						.removeClass("mbsc-textarea-scroll"), t && (e.style.height = t + "px"))
				}

				function h() {
					clearTimeout(b), b = setTimeout(function() {
						i("textarea.mbsc-control", x).each(function() {
							u(this)
						})
					}, 100)
				}

				function f(e) {
					return !(!e.id || !n.instances[e.id])
				}
				var p, b, v, g = "",
					x = i(e),
					T = this;
				n.classes.Base.call(this, e, c, !0), T.refresh = function(e) {
					i("input,select,textarea,progress,button", x).each(function() {
						function s() {
							i("input", S).val(-1 != C.selectedIndex ? C.options[C.selectedIndex]
								.text : "")
						}
						var c, h, g, y, w, C = this,
							M = i(C),
							S = M.parent(),
							D = M.attr("data-role"),
							k = M.attr("type") || C.nodeName.toLowerCase();
						if ("false" != M.attr("data-enhance") && t.vKMaI) {
							if (/(switch|range|segmented|stepper)/.test(D) && (k = D), M.hasClass(
									"mbsc-control")) /(switch|range|progress)/.test(k) && f(C) && !
								e && n.instances[C.id].option({
									theme: p.theme,
									lang: p.lang,
									onText: p.onText,
									offText: p.offText,
									stopProp: p.stopProp
								});
							else switch ("button" != k && "submit" != k && "segmented" != k && (S
								.find("label").addClass("mbsc-label"), S.contents().filter(
									function() {
										return 3 == this.nodeType && this.nodeValue && /\S/
											.test(this.nodeValue)
									}).each(function() {
									i('<span class="mbsc-label"></span>').insertAfter(
										this).append(this)
								})), M.addClass("mbsc-control"), k) {
								case "button":
								case "submit":
									h = M.attr("data-icon"), M.addClass("mbsc-btn"), h && (M
										.prepend(
											'<span class="mbsc-btn-ic mbsc-ic mbsc-ic-' +
											h + '"></span>'), "" === M.text() && M.addClass(
											"mbsc-btn-icon-only"));
									break;
								case "switch":
									f(C) || new n.classes.Switch(C, {
										theme: p.theme,
										lang: p.lang,
										rtl: p.rtl,
										onText: p.onText,
										offText: p.offText,
										stopProp: p.stopProp
									});
									break;
								case "checkbox":
									S.prepend(M).addClass("mbsc-checkbox mbsc-control-w"), M
										.after('<span class="mbsc-checkbox-box"></span>');
									break;
								case "range":
									S.hasClass("mbsc-slider") || f(C) || new n.classes.Slider(
									C, {
										theme: p.theme,
										lang: p.lang,
										rtl: p.rtl,
										stopProp: p.stopProp
									});
									break;
								case "progress":
									f(C) || new n.classes.Progress(C, {
										theme: p.theme,
										lang: p.lang,
										rtl: p.rtl
									});
									break;
								case "radio":
									S.addClass("mbsc-radio mbsc-control-w"), M.after(
										'<span class="mbsc-radio-box"><span></span></span>');
									break;
								case "select":
								case "select-one":
								case "select-multiple":
									c = M.prev().is("input.mbsc-control") ? M.prev() : i(
											'<input tabindex="-1" type="text" class="mbsc-control mbsc-control-ev" readonly>'
											), m(M), S.addClass(
											"mbsc-input mbsc-select mbsc-control-w"), M.after(
										c), c.after(
											'<span class="mbsc-select-ic mbsc-ic mbsc-ic-arrow-down5"></span>'
											);
									break;
								case "textarea":
									m(M), S.addClass("mbsc-input mbsc-textarea mbsc-control-w");
									break;
								case "segmented":
									var _, A;
									M.parent().hasClass("mbsc-segmented-item") || (A = i(
											'<div class="mbsc-segmented"></div>'), S.after(
											A), i('input[name="' + M.attr("name") + '"]', x)
										.each(function(e, t) {
											_ = i(t).parent().addClass(
													"mbsc-segmented-item"), i(
													'<span class="mbsc-segmented-content">' +
													(i(t).attr("data-icon") ?
														' <span class="mbsc-ic mbsc-ic-' +
														i(t).attr("data-icon") +
														'"></span> ' : "") + "</span>")
												.append(_.contents()).appendTo(_), _
												.prepend(t), A.append(_)
										}));
									break;
								case "stepper":
									f(C) || new n.classes.Stepper(C, {
										form: T
									});
									break;
								case "hidden":
									break;
								default:
									m(M), S.addClass("mbsc-input mbsc-control-w")
							}
							M.hasClass("mbsc-control-ev") || (/select/.test(k) && !M.hasClass(
									"mbsc-comp") && (M.on("change.mbsc-form", s), s()),
								"textarea" == k && M.on("keydown.mbsc-form input.mbsc-form",
									function() {
										clearTimeout(b), b = setTimeout(function() {
											u(C)
										}, 100)
									}).on("scroll.mbsc-form", d), M.addClass("mbsc-control-ev")
								.on("touchstart.mbsc-form mousedown.mbsc-form", function(e) {
									l(e, this) && (y = o(e, "X"), w = o(e, "Y"), a && a
										.removeClass("mbsc-active"), C.disabled || (
											g = !0, a = i(this), i(this).addClass(
												"mbsc-active"), v("onControlActivate", {
												target: this,
												domEvent: e
											})))
								}).on("touchmove.mbsc-form mousemove.mbsc-form", function(e) {
									(g && Math.abs(o(e, "X") - y) > 9 || Math.abs(o(e,
										"Y") - w) > 9) && (M.removeClass("mbsc-active"), v(
										"onControlDeactivate", {
											target: M[0],
											domEvent: e
										}), g = !1)
								}).on(
									"touchend.mbsc-form touchcancel.mbsc-form mouseleave.mbsc-form mouseup.mbsc-form",
									function(e) {
										if (g && "touchend" == e.type && !C.readOnly && (C
												.focus(),
												/(button|submit|checkbox|switch|radio)/.test(
												k) && e.preventDefault(), !/select/.test(k))) {
											var t = (e.originalEvent || e).changedTouches[0],
												s = document.createEvent("MouseEvents");
											s.initMouseEvent("click", !0, !0, window, 1, t
													.screenX, t.screenY, t.clientX, t.clientY, !
													1, !1, !1, !1, 0, null), s.tap = !0, C
												.dispatchEvent(s), r.preventClick()
										}
										g && setTimeout(function() {
											M.removeClass("mbsc-active"), v(
												"onControlDeactivate", {
													target: M[0],
													domEvent: e
												})
										}, 100), g = !1, a = null
									}))
						}
					}), e || h()
				}, T.init = function(e) {
					T._init(e), n.themes.form[p.theme] || (p.theme = "mobiscroll"), x.hasClass(
						"mbsc-form") || (x.on("touchstart", s).show(), i(window).on(
							"resize orientationchange", h)), g && x.removeClass(g), g = "mbsc-form mbsc-" +
						p.theme + (p.baseTheme ? " mbsc-" + p.baseTheme : "") + (p.rtl ? " mbsc-rtl" :
							" mbsc-ltr"), x.addClass(g), T.refresh(), T.trigger("onInit")
				}, T.destroy = function() {
					x.removeClass(g).off("touchstart", s), i(window).off("resize orientationchange", h), i(
							".mbsc-control", x).off(".mbsc-form").removeClass("mbsc-control-ev"), T
						._destroy(), i(".mbsc-progress progress", x).mobiscroll("destroy"), i(
							".mbsc-slider input", x).mobiscroll("destroy"), i(".mbsc-stepper input", x)
						.mobiscroll("destroy"), i(".mbsc-switch input", x).mobiscroll("destroy")
				}, p = T.settings, v = T.trigger, T.init(c)
			}, n.classes.Form.prototype = {
				_hasDef: !0,
				_hasTheme: !0,
				_hasLang: !0,
				_class: "form",
				_defaults: {
					tap: !0,
					stopProp: !0,
					lang: "en"
				}
			}, n.themes.form.mobiscroll = {}, n.presetShort("form", "Form"), n.classes.Stepper = function(t,
			a) {
				function s(e) {
					32 == e.keyCode && (e.preventDefault(), y || t.disabled || (v = i(this).addClass(
						"mbsc-active"), f(e)))
				}

				function r(e) {
					y && (e.preventDefault(), p(!0))
				}

				function c(e) {
					l(e, this) && !t.disabled && (v = i(this).addClass("mbsc-active").trigger("focus"), j && j
						.trigger("onControlActivate", {
							target: v[0],
							domEvent: e
						}), f(e), "mousedown" === e.type && i(document).on("mousemove", d).on("mouseup", m))
				}

				function m(e) {
					y && (e.preventDefault(), p(!0, e), "mouseup" === e.type && i(document).off("mousemove", d)
						.off("mouseup", m))
				}

				function d(e) {
					y && (D = o(e, "X"), k = o(e, "Y"), C = D - L, M = k - I, (Math.abs(C) > 7 || Math.abs(M) >
						7) && p())
				}

				function u() {
					var e;
					t.disabled || (e = parseFloat(i(this).val()), h(isNaN(e) ? H : e))
				}

				function h(t, a, s) {
					z = H, a === e && (a = !0), s === e && (s = a), H = t !== e ? Math.min(A, Math.max(Math
							.round(t / N) * N, V)) : Math.min(A, Math.max(H + (v.hasClass(
							"mbsc-stepper-minus") ? -N : N), V)), w = !0, T.removeClass("mbsc-step-disabled"),
						a && Y.val(H), H == V ? x.addClass("mbsc-step-disabled") : H == A && g.addClass(
							"mbsc-step-disabled"), H !== z && s && Y.trigger("change")
				}

				function f(e) {
					y || (y = !0, w = !1, L = o(e, "X"), I = o(e, "Y"), clearInterval(_), clearTimeout(_), _ =
						setTimeout(function() {
							h(), _ = setInterval(function() {
								h()
							}, 150)
						}, 300))
				}

				function p(e, t) {
					clearInterval(_), clearTimeout(_), !w && e && h(), y = !1, w = !1, v.removeClass(
						"mbsc-active"), j && setTimeout(function() {
						j.trigger("onControlDeactivate", {
							target: v[0],
							domEvent: t
						})
					}, 100)
				}

				function b(t, a) {
					var s = Y.attr(t);
					return s === e || "" === s ? a : +s
				}
				var v, g, x, T, y, w, C, M, S, D, k, _, A, V, N, F, L, I, H, O, P, E = this,
					Y = i(t),
					z = H,
					j = a.form;
				n.classes.Base.call(this, t, a, !0), E.getVal = function() {
					var e = parseFloat(Y.val());
					return e = isNaN(e) ? H : e, Math.min(A, Math.max(Math.round(e / N) * N, V))
				}, E.setVal = function(e, t, a) {
					e = parseFloat(e), h(isNaN(e) ? H : e, t, a)
				}, E.init = function(a) {
					O = Y.parent().hasClass("mbsc-stepper"), P = O ? Y.closest(".mbsc-stepper-cont") : Y
						.parent(), E._init(a), F = E.settings, V = a.min === e ? b("min", F.min) : a.min,
						A = a.max === e ? b("max", F.max) : a.max, N = a.step === e ? b("step", F.step) : a
						.step, S = Y.attr("data-val") || F.val, H = Math.min(A, Math.max(Math.round(+t
							.value / N) * N || 0, V)), O || P.addClass("mbsc-stepper-cont mbsc-control-w")
						.append('<span class="mbsc-segmented mbsc-stepper"></span>').find(".mbsc-stepper")
						.append(
							'<span class="mbsc-segmented-item mbsc-stepper-control mbsc-stepper-minus ' + (
								H == V ? "mbsc-step-disabled" : "") +
							'"  tabindex="0"><span class="mbsc-segmented-content"><span class="mbsc-ic mbsc-ic-minus"></span></span></span>'
							).append(
							'<span class="mbsc-segmented-item mbsc-stepper-control mbsc-stepper-plus ' + (
								H == A ? "mbsc-step-disabled" : "") +
							'"  tabindex="0"><span class="mbsc-segmented-content"> <span class="mbsc-ic mbsc-ic-plus"></span> </span></span>'
							).prepend(Y), x = i(".mbsc-stepper-minus", P), g = i(".mbsc-stepper-plus", P),
						O || ("left" == S ? (P.addClass("mbsc-stepper-val-left"), Y.after(
							'<span class="mbsc-segmented-item"><span class="mbsc-segmented-content"></span></span>'
							)) : "right" == S ? (P.addClass("mbsc-stepper-val-right"), g.after(
							'<span class="mbsc-segmented-item"><span class="mbsc-segmented-content"></span></span>'
							)) : x.after(
							'<span class="mbsc-segmented-item"><span class="mbsc-segmented-content mbsc-stepper-val"></span></span>'
							)), Y.val(H).attr("data-role", "stepper").attr("min", V).attr("max", A).attr(
							"step", N).on("change", u), T = i(".mbsc-stepper-control", P).on("keydown", s)
						.on("keyup", r).on("mousedown touchstart", c).on("touchmove", d).on(
							"touchend touchcancel", m), Y.addClass("mbsc-stepper-ready mbsc-control")
				}, E.destroy = function() {
					Y.removeClass("mbsc-control").off("change", u), T.off("keydown", s).off("keyup", r).off(
							"mousedown touchstart", c).off("touchmove", d).off("touchend touchcancel", m), E
						._destroy()
				}, E.init(a)
			}, n.classes.Stepper.prototype = {
				_class: "stepper",
				_defaults: {
					min: 0,
					max: 100,
					step: 1
				}
			}, n.presetShort("stepper", "Stepper"), n.classes.Switch = function(e, t) {
				var a, s, r, o = this;
				t = t || {}, i.extend(t, {
					changeEvent: "click",
					min: 0,
					max: 1,
					step: 1,
					live: !1,
					round: !1,
					handle: !1,
					highlight: !1
				}), n.classes.Slider.call(this, e, t, !0), o._readValue = function() {
					return e.checked ? 1 : 0
				}, o._fillValue = function(e, t, s) {
					a.prop("checked", !!e), s && a.trigger("change")
				}, o._onTap = function(e) {
					o._setVal(e ? 0 : 1)
				}, o.__onInit = function() {
					r = o.settings, a = i(e), s = a.parent(), s.find(".mbsc-switch-track").remove(), s
						.prepend(a), a.attr("data-role", "switch").after(
							'<span class="mbsc-progress-cont mbsc-switch-track"><span class="mbsc-progress-track mbsc-progress-anim"><span class="mbsc-slider-handle-cont"><span class="mbsc-slider-handle mbsc-switch-handle" data-index="0"><span class="mbsc-switch-txt-off">' +
							r.offText + '</span><span class="mbsc-switch-txt-on">' + r.onText +
							"</span></span></span></span></span>"), o._$track = s.find(
							".mbsc-progress-track")
				}, o.getVal = function() {
					return e.checked
				}, o.setVal = function(e, t, a) {
					o._setVal(e ? 1 : 0, t, a)
				}, o.init(t)
			}, n.classes.Switch.prototype = {
				_class: "switch",
				_css: "mbsc-switch",
				_hasTheme: !0,
				_hasLang: !0,
				_defaults: {
					stopProp: !0,
					offText: "Off",
					onText: "On"
				}
			}, n.presetShort("switch", "Switch"), i(function() {
				i("[mbsc-enhance]").each(function() {
					i(this).mobiscroll().form()
				}), i(document).on("mbsc-enhance", function(e, t) {
					i(e.target).is("[mbsc-enhance]") ? i(e.target).mobiscroll().form(t) : i(
						"[mbsc-enhance]", e.target).each(function() {
						i(this).mobiscroll().form(t)
					})
				}), i(document).on("mbsc-refresh", function(e) {
					i(e.target).is("[mbsc-enhance]") ? i(e.target).mobiscroll("refresh") : i(
						"[mbsc-enhance]", e.target).each(function() {
						i(this).mobiscroll("refresh")
					})
				})
			})
		}(),
		function() {
			t.themes.form["android-holo"] = {}
		}(),
		function() {
			t.themes.form.ios = {}
		}(),
		function() {
			var e = t.$;
			t.themes.form.material = {
				onControlActivate: function(a) {
					var s, n = e(a.target);
					("button" == n[0].type || "submit" == n[0].type) && (s = n), "segmented" == n.attr(
							"data-role") && (s = n.next()), n.hasClass("mbsc-stepper-control") && !n
						.hasClass("mbsc-step-disabled") && (s = n.find(".mbsc-segmented-content")), s && t
						.themes.material.addRipple(s, a.domEvent)
				},
				onControlDeactivate: function() {
					t.themes.material.removeRipple()
				}
			}
		}(),
		function() {
			t.themes.form.wp = {}
		}(),
		function() {
			var e = t,
				a = e.$;
			e.themes.frame.bootstrap = {
				dateDisplay: "Mddyy",
				disabledClass: "disabled",
				activeClass: "btn-primary",
				activeTabClass: "active",
				todayClass: "text-primary",
				btnCalPrevClass: "",
				btnCalNextClass: "",
				onMarkupInserted: function(e) {
					var t = a(e.target);
					a(".mbsc-fr-popup", t).addClass("popover"), a(".mbsc-fr-w", t).addClass(
							"popover-content"), a(".mbsc-fr-hdr", t).addClass("popover-title"), a(
							".mbsc-fr-arr-i", t).addClass("popover"), a(".mbsc-fr-arr", t).addClass(
						"arrow"), a(".mbsc-fr-btn", t).addClass("btn btn-default"), a(
							".mbsc-fr-btn-s .mbsc-fr-btn", t).removeClass("btn-default").addClass(
							"btn btn-primary"), a(".mbsc-sc-btn-plus", t).addClass(
							"glyphicon glyphicon-chevron-down"), a(".mbsc-sc-btn-minus", t).addClass(
							"glyphicon glyphicon-chevron-up"), a(".mbsc-cal-next .mbsc-cal-btn-txt", t)
						.prepend('<i class="glyphicon glyphicon-chevron-right"></i>'), a(
							".mbsc-cal-prev .mbsc-cal-btn-txt", t).prepend(
							'<i class="glyphicon glyphicon-chevron-left"></i>'), a(".mbsc-cal-tabs ul", t)
						.addClass("nav nav-tabs"), a(".mbsc-cal-sc-c", t).addClass("popover"), a(
							".mbsc-cal-week-nrs-c", t).addClass("popover"), a(".mbsc-cal-events", t)
						.addClass("popover"), a(".mbsc-cal-events-arr", t).addClass("arrow"), a(
							".mbsc-range-btn", t).addClass("btn btn-sm btn-small btn-default"), a(
							".mbsc-np-btn", t).addClass("btn btn-default")
				},
				onPosition: function(e) {
					setTimeout(function() {
						a(".mbsc-fr-bubble-top, .mbsc-fr-bubble-top .mbsc-fr-arr-i", e.target)
							.removeClass("bottom").addClass("top"), a(
								".mbsc-fr-bubble-bottom, .mbsc-fr-bubble-bottom .mbsc-fr-arr-i", e
								.target).removeClass("top").addClass("bottom")
					}, 10)
				},
				onEventBubbleShow: function(e) {
					var t = a(e.eventList);
					a(".mbsc-cal-event-list", t).addClass("list-group"), a(".mbsc-cal-event", t).addClass(
						"list-group-item"), setTimeout(function() {
						t.hasClass("mbsc-cal-events-b") ? t.removeClass("top").addClass("bottom") :
							t.removeClass("bottom").addClass("top")
					}, 10)
				}
			}
		}(),
		function() {
			var e = t,
				a = e.$;
			e.themes.frame.material = {
				showLabel: !1,
				headerText: !1,
				btnWidth: !1,
				selectedLineBorder: 2,
				weekDays: "min",
				deleteIcon: "material-backspace",
				icon: {
					filled: "material-star",
					empty: "material-star-outline"
				},
				checkIcon: "material-check",
				btnPlusClass: "mbsc-ic mbsc-ic-material-keyboard-arrow-down",
				btnMinusClass: "mbsc-ic mbsc-ic-material-keyboard-arrow-up",
				btnCalPrevClass: "mbsc-ic mbsc-ic-material-keyboard-arrow-left",
				btnCalNextClass: "mbsc-ic mbsc-ic-material-keyboard-arrow-right",
				onMarkupReady: function(t) {
					e.themes.material.initRipple(a(t.target), ".mbsc-fr-btn-e", "mbsc-fr-btn-d",
						"mbsc-fr-btn-nhl")
				},
				onEventBubbleShow: function(e) {
					var t = a(e.eventList),
						s = a(e.target).closest(".mbsc-cal-row").index() < 2,
						n = a(".mbsc-cal-event-color", t).eq(s ? 0 : -1).css("background-color");
					a(".mbsc-cal-events-arr", t).css("border-color", s ? "transparent transparent " + n +
						" transparent" : n + "transparent transparent transparent")
				}
			}
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = a.util,
				i = n.isNumeric,
				r = function() {},
				o = a.classes;
			o.Numpad = function(a, n, r) {
				function l(e) {
					var n, i = g.validate.call(a, {
							values: T.slice(0),
							variables: A
						}, D) || [],
						r = i && i.disabled || [];
					if (D._isValid = i.invalid ? !1 : !0, D._tempValue = g.formatValue.call(a, T.slice(0), A,
						D), v = T.length, y = i.length || C, D._isVisible && t.vKMaI) {
						if (s(".mbsc-np-ph", f).each(function(e) {
								s(this).html("ltr" == g.fill ? e >= v ? b : x || T[e] : e >= C - y ? C > e +
									v ? b : x || T[e + v - C] : "")
							}), s(".mbsc-np-cph", f).each(function() {
								s(this).html(A[s(this).attr("data-var")] || s(this).attr("data-ph"))
							}), v === C)
							for (n = 0; 9 >= n; n++) r.push(n);
						for (s(".mbsc-np-btn", f).removeClass(p), n = 0; n < r.length; n++) s(
							'.mbsc-np-btn[data-val="' + r[n] + '"]', f).addClass(p);
						D._isValid ? s(".mbsc-fr-btn-s .mbsc-fr-btn", f).removeClass(p) : s(
							".mbsc-fr-btn-s .mbsc-fr-btn", f).addClass(p), D.live && (D._hasValue = e || D
							._hasValue, c(e, !1, e), e && w("onSet", {
								valueText: D._value
							}))
					}
				}

				function c(e, t, a, n) {
					t && l(), n || (M = T.slice(0), V = s.extend({}, A), k = _.slice(0), D._value = D
						._hasValue ? D._tempValue : null), e && (D._isInput && S.val(D._hasValue && D
						._isValid ? D._value : ""), w("onFill", {
						valueText: D._hasValue ? D._tempValue : "",
						change: a
					}), a && (D._preventChange = !0, S.trigger("change")))
				}

				function m(e) {
					var t, a, s = e || [],
						n = [];
					for (_ = [], A = {}, t = 0; t < s.length; t++) /:/.test(s[t]) ? (a = s[t].split(":"), A[a[
						0]] = a[1], _.push(a[0])) : (n.push(s[t]), _.push("digit"));
					return n
				}

				function d(e, t) {
					!(v || t || g.allowLeadingZero) || e.hasClass("mbsc-fr-btn-d") || e.hasClass(
						"mbsc-np-btn-empty") || C > v && (_.push("digit"), T.push(t), l(!0))
				}

				function u(e, t) {
					var a, s, n = e.attr("data-var");
					if (!e.hasClass("mbsc-fr-btn-d")) {
						if (n && (s = n.split(":"), _.push(s[0]), A[s[0]] = s[1]), t.length + v <= y)
							for (a = 0; a < t.length; ++a) _.push("digit"), T.push(i(t[a]) ? +t[a] : t[a]);
						l(!0)
					}
				}

				function h() {
					var e, t, a = _.pop();
					if (v || "digit" !== a) {
						if ("digit" !== a && A[a])
							for (delete A[a], t = _.slice(0), _ = [], e = 0; e < t.length; e++) t[e] !== a && _
								.push(t[e]);
						else T.pop();
						l(!0)
					}
				}
				var f, p, b, v, g, x, T, y, w, C, M, S = s(a),
					D = this,
					k = [],
					_ = [],
					A = {},
					V = {},
					N = {
						48: 0,
						49: 1,
						50: 2,
						51: 3,
						52: 4,
						53: 5,
						54: 6,
						55: 7,
						56: 8,
						57: 9,
						96: 0,
						97: 1,
						98: 2,
						99: 3,
						100: 4,
						101: 5,
						102: 6,
						103: 7,
						104: 8,
						105: 9
					};
				o.Frame.call(this, a, n, !0), D.setVal = D._setVal = function(t, n, i, r) {
					D._hasValue = null !== t && t !== e, T = m(s.isArray(t) ? t.slice(0) : g.parseValue
						.call(a, t, D)), c(n, !0, i === e ? n : i, r)
				}, D.getVal = D._getVal = function(e) {
					return D._hasValue || e ? D[e ? "_tempValue" : "_value"] : null
				}, D.setArrayVal = D.setVal, D.getArrayVal = function(e) {
					return e ? T.slice(0) : D._hasValue ? M.slice(0) : null
				}, D._readValue = function() {
					var e = S.val() || "";
					"" !== e && (D._hasValue = !0), x ? (A = {}, _ = [], T = []) : (A = D._hasValue ? V :
						{}, _ = D._hasValue ? k : [], T = D._hasValue && M ? M.slice(0) : m(g.parseValue
							.call(a, e, D)), c(!1, !0))
				}, D._fillValue = function() {
					D._hasValue = !0, c(!0, !1, !0)
				}, D._generateContent = function() {
					var e, t, a, s = 1,
						n = "",
						i = "";
					for (i += '<div class="mbsc-np-hdr"><div role="button" tabindex="0" aria-label="' + g
						.deleteText + '" class="mbsc-np-del mbsc-fr-btn-e mbsc-ic mbsc-ic-' + g.deleteIcon +
						'"></div><div class="mbsc-np-dsp">', n = g.template.replace(/d/g,
							'<span class="mbsc-np-ph">' + b + "</span>").replace(/&#100;/g, "d"), n = n
						.replace(/{([a-zA-Z0-9]*)\:?([a-zA-Z0-9\-\_]*)}/g,
							'<span class="mbsc-np-cph" data-var="$1" data-ph="$2">$2</span>'), i += n, i +=
						"</div></div>", i +=
						'<div class="mbsc-np-tbl-c mbsc-w-p"><div class="mbsc-np-tbl">', e = 0; 4 > e; e++
						) {
						for (i += '<div class="mbsc-np-row">', t = 0; 3 > t; t++) a = s, 10 == s || 12 ==
							s ? a = "" : 11 == s && (a = 0), i += "" === a ? 10 == s && g.leftKey ?
							'<div role="button" tabindex="0" class="mbsc-np-btn mbsc-np-btn-custom mbsc-fr-btn-e" ' +
							(g.leftKey.variable ? 'data-var="' + g.leftKey.variable + '"' : "") +
							' data-val="' + (g.leftKey.value || "") + '" >' + g.leftKey.text + "</div>" :
							12 == s && g.rightKey ?
							'<div role="button" tabindex="0" class="mbsc-np-btn mbsc-np-btn-custom mbsc-fr-btn-e" ' +
							(g.rightKey.variable ? 'data-var="' + g.rightKey.variable + '"' : "") +
							' data-val="' + (g.rightKey.value || "") + '" >' + g.rightKey.text + "</div>" :
							'<div class="mbsc-np-btn mbsc-np-btn-empty"></div>' :
							'<div tabindex="0" role="button" class="mbsc-np-btn mbsc-fr-btn-e" data-val="' +
							a + '">' + a + "</div>", s++;
						i += "</div>"
					}
					return i += "</div></div>"
				}, D._markupReady = function() {
					f = D._markup, l()
				}, D._attachEvents = function(t) {
					t.on("keydown", function(t) {
						N[t.keyCode] !== e ? d(s('.mbsc-np-btn[data-val="' + N[t.keyCode] + '"]'),
							N[t.keyCode]) : 8 == t.keyCode && (t.preventDefault(), h())
					}), D.tap(s(".mbsc-np-btn", t), function() {
						var e = s(this);
						e.hasClass("mbsc-np-btn-custom") ? u(e, e.attr("data-val")) : d(e, +e.attr(
							"data-val"))
					}, e, 30), D.tap(s(".mbsc-np-del", t), h, e, 30)
				}, D._processSettings = function() {
					g = D.settings, g.headerText = (g.headerText || "").replace("{value}", ""), g.cssClass =
						(g.cssClass || "") + " mbsc-np", g.template = g.template.replace(/\\d/, "&#100;"),
						b = g.placeholder, C = (g.template.match(/d/g) || []).length, p = "mbsc-fr-btn-d " +
						(g.disabledClass || ""), x = g.mask, w = D.trigger, x && S.is("input") && S.attr(
							"type", "password")
				}, D._indexOf = function(e, t) {
					var a;
					for (a = 0; a < e.length; ++a)
						if (e[a].toString() === t.toString()) return a;
					return -1
				}, r || D.init(n)
			}, o.Numpad.prototype = {
				_hasDef: !0,
				_hasTheme: !0,
				_hasLang: !0,
				_hasPreset: !0,
				_class: "numpad",
				_defaults: s.extend({}, o.Frame.prototype._defaults, {
					template: "dd.dd",
					placeholder: "0",
					deleteIcon: "backspace",
					allowLeadingZero: !1,
					fill: "rtl",
					deleteText: "Delete",
					decimalSeparator: ".",
					thousandsSeparator: ",",
					validate: r,
					parseValue: r,
					formatValue: function(e, t, a) {
						var n, i = 1,
							r = a.settings,
							o = r.placeholder,
							l = r.template,
							c = e.length,
							m = l.length,
							d = "";
						for (n = 0; m > n; n++) "d" == l[m - n - 1] ? (d = c >= i ? e[c - i] + d :
							o + d, i++) : d = l[m - n - 1] + d;
						return s.each(t, function(e, t) {
							d = d.replace("{" + e + "}", t)
						}), s("<div>" + d + "</div>").text()
					}
				})
			}, a.themes.numpad = a.themes.frame, a.presetShort("numpad", "Numpad", !1)
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.numpad,
				n = {
					min: 0,
					max: 99.99,
					scale: 2,
					prefix: "",
					suffix: "",
					returnAffix: !1
				};
			s.decimal = function(t) {
				function s(e) {
					for (var t, a = e.slice(0), s = 0; a.length;) s = 10 * s + a.shift();
					for (t = 0; t < o.scale; t++) s /= 10;
					return s
				}

				function i(e) {
					var t = s(e).toFixed(o.scale).replace(".", o.decimalSeparator).replace(
						/\B(?=(\d{3})+(?!\d))/g, o.thousandsSeparator);
					return t
				}
				var r = a.extend({}, t.settings),
					o = a.extend(t.settings, n, r);
				return t.getVal = function(a) {
					var s = t._getVal(a);
					return e.util.isNumeric(s) ? +s : s
				}, {
					template: o.prefix.replace(/d/g, "\\d") + Array((Math.floor(o.max) + "").length + 1)
						.join("d") + (o.scale ? "." + Array(o.scale + 1).join("d") : "") + o.suffix.replace(
							/d/g, "\\d"),
					parseValue: function(e) {
						var t, a, s = e || o.defaultValue,
							n = [];
						if (s && (s += "", a = s.match(/\d+\.?\d*/g)))
							for (a = (+a[0]).toFixed(o.scale), t = 0; t < a.length; t++) "." != a[t] &&
								(+a[t] ? n.push(+a[t]) : n.length && n.push(0));
						return n
					},
					formatValue: function(e) {
						var t = i(e);
						return o.returnAffix ? o.prefix + t + o.suffix : t
					},
					validate: function(e) {
						var n = e.values,
							r = i(n),
							l = s(n),
							c = [];
						return n.length || o.allowLeadingZero || c.push(0), t.isVisible() && a(
							".mbsc-np-dsp", t._markup).html(o.prefix + r + o.suffix), {
							disabled: c,
							invalid: l > o.max || l < o.min || (o.invalid ? -1 != t._indexOf(o
								.invalid, l) : !1)
						}
					}
				}
			}
		}(),
		function() {
			function e(e) {
				for (var t = 0, a = 1, s = 0; e.length;) t > 3 ? a = 3600 : t > 1 && (a = 60), s += e.pop() * a * (
					t % 2 ? 10 : 1), t++;
				return s
			}
			var a = t,
				s = a.$,
				n = a.presets.numpad,
				i = ["h", "m", "s"],
				r = {
					min: 0,
					max: 362439,
					defaultValue: 0,
					hourTextShort: "h",
					minuteTextShort: "m",
					secTextShort: "s"
				};
			n.timespan = function(t) {
				function n(e) {
					var t, a = "",
						n = 3600;
					return s(i).each(function(s, i) {
						t = Math.floor(e / n), e -= t * n, n /= 60, (t > 0 || "s" == i && !a) && (a =
							a + (a ? " " : "") + t + c[i])
					}), a
				}
				var o = s.extend({}, t.settings),
					l = s.extend(t.settings, r, o),
					c = {
						h: l.hourTextShort.replace(/d/g, "\\d"),
						m: l.minuteTextShort.replace(/d/g, "\\d"),
						s: l.secTextShort.replace(/d/g, "\\d")
					},
					m = 'd<span class="mbsc-np-sup mbsc-np-time">' + c.s + "</span>";
				return l.max > 9 && (m = "d" + m), l.max > 99 && (m = '<span class="mbsc-np-ts-m">' + (l.max >
							639 ? "d" : "") + 'd</span><span class="mbsc-np-sup mbsc-np-time">' + c.m +
						"</span>" + m), l.max > 6039 && (m = '<span class="mbsc-np-ts-h">' + (l.max > 38439 ?
						"d" : "") + 'd</span><span class="mbsc-np-sup mbsc-np-time">' + c.h + "</span>" + m), t
					.setVal = function(e, s, i, r) {
						return a.util.isNumeric(e) && (e = n(e)), t._setVal(e, s, i, r)
					}, t.getVal = function(a) {
						return t._hasValue || a ? e(t.getArrayVal(a)) : null
					}, {
						template: m,
						parseValue: function(e) {
							var t, a = e || n(l.defaultValue),
								r = [];
							return a && s(i).each(function(e, s) {
								t = new RegExp("(\\d+)" + c[s], "gi").exec(a), t ? (t = +t[1], t >
										9 ? (r.push(Math.floor(t / 10)), r.push(t % 10)) : (r
											.length && r.push(0), (t || r.length) && r.push(t))) : r
									.length && (r.push(0), r.push(0))
							}), r
						},
						formatValue: function(t) {
							return n(e(t))
						},
						validate: function(a) {
							var s = a.values,
								n = e(s.slice(0)),
								i = [];
							return s.length || i.push(0), {
								disabled: i,
								invalid: n > l.max || n < l.min || (l.invalid ? -1 != t._indexOf(l
									.invalid, +n) : !1)
							}
						}
					}
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.numpad,
				n = {
					timeFormat: "hh:ii A",
					amText: "am",
					pmText: "pm"
				};
			s.time = function(e) {
				function t(e, t) {
					var s, n = "";
					for (s = 0; s < e.length; ++s) n += e[s] + (s % 2 == (e.length % 2 == 1 ? 0 : 1) && s != e
						.length - 1 ? ":" : "");
					return a.each(t, function(e, t) {
						n += " " + t
					}), n
				}

				function s(e) {
					var t, a, s, n, i, c, m, g, x, T, y = [],
						w = 2 * o.length;
					if (d = w, e.length || (l && (y.push(0), y.push(r.leftKey.value)), y.push(r.rightKey
						.value)), !l && (w - e.length < 2 || 1 != e[0] && (e[0] > 2 || e[1] > 3) && w - e
							.length <= 2) && (y.push("30"), y.push("00")), (l ? e[0] > 1 || e[1] > 2 : 1 != e[
							0] && (e[0] > 2 || e[1] > 3)) && e[0] && (e.unshift(0), d = w - 1), e.length == w)
						for (t = 0; 9 >= t; ++t) y.push(t);
					else if (1 == e.length && l && 1 == e[0] || e.length && e.length % 2 === 0 || !l && 2 == e[
							0] && e[1] > 3 && e.length % 2 == 1)
						for (t = 6; 9 >= t; ++t) y.push(t);
					if (x = void 0 !== e[1] ? "" + e[0] + e[1] : "", T = +p == +(void 0 !== e[3] ? "" + e[2] +
							e[3] : ""), r.invalid)
						for (t = 0; t < r.invalid.length; ++t)
							if (c = r.invalid[t].getHours(), m = r.invalid[t].getMinutes(), g = r.invalid[t]
								.getSeconds(), c == +x) {
								if (2 == o.length && (10 > m ? 0 : +("" + m)[0]) == +e[2]) {
									y.push(10 > m ? m : +("" + m)[1]);
									break
								}
								if ((10 > g ? 0 : +("" + g)[0]) == +e[4]) {
									y.push(10 > g ? g : +("" + g)[1]);
									break
								}
							} if (r.min || r.max) {
						if (a = +u == +x, s = +h == +x, i = s && T, n = a && T, 0 === e.length) {
							for (t = l ? 2 : u > 19 ? u[0] : 3; t <= (1 == u[0] ? 9 : u[0] - 1); ++t) y.push(t);
							if (u >= 10 && (y.push(0), 2 == u[0]))
								for (t = 3; 9 >= t; ++t) y.push(t);
							if (h && 10 > h || u && u >= 10)
								for (t = h && 10 > h ? +h[0] + 1 : 0; t < (u && u >= 10 ? u[0] : 10); ++t) y
									.push(t)
						}
						if (1 == e.length) {
							if (0 === e[0])
								for (t = 0; t < u[0]; ++t) y.push(t);
							if (u && 0 !== e[0] && (l ? 1 == e[0] : 2 == e[0]))
								for (t = l ? 3 : 4; 9 >= t; ++t) y.push(t);
							if (e[0] == u[0])
								for (t = 0; t < u[1]; ++t) y.push(t);
							if (e[0] == h[0] && !l)
								for (t = +h[1] + 1; 9 >= t; ++t) y.push(t)
						}
						if (2 == e.length && (a || s))
							for (t = s ? +p[0] + 1 : 0; t < (a ? +f[0] : 10); ++t) y.push(t);
						if (3 == e.length && (s && e[2] == p[0] || a && e[2] == f[0]))
							for (t = s && e[2] == p[0] ? +p[1] + 1 : 0; t < (a && e[2] == f[0] ? +f[1] : 10); ++
								t) y.push(t);
						if (4 == e.length && (n || i))
							for (t = i ? +v[0] + 1 : 0; t < (n ? +b[0] : 10); ++t) y.push(t);
						if (5 == e.length && (n && e[4] == b[0] || i && e[4] == v[0]))
							for (t = i && e[4] == v[0] ? +v[1] + 1 : 0; t < (n && e[4] == b[0] ? +b[1] : 10); ++
								t) y.push(t)
					}
					return y
				}
				var i = a.extend({}, e.settings),
					r = a.extend(e.settings, n, i),
					o = r.timeFormat.split(":"),
					l = r.timeFormat.match(/a/i),
					c = l ? "a" == l[0] ? r.amText : r.amText.toUpperCase() : "",
					m = l ? "a" == l[0] ? r.pmText : r.pmText.toUpperCase() : "",
					d = 0,
					u = r.min ? "" + r.min.getHours() : "",
					h = r.max ? "" + r.max.getHours() : "",
					f = r.min ? "" + (r.min.getMinutes() < 10 ? "0" + r.min.getMinutes() : r.min.getMinutes()) :
					"",
					p = r.max ? "" + (r.max.getMinutes() < 10 ? "0" + r.max.getMinutes() : r.max.getMinutes()) :
					"",
					b = r.min ? "" + (r.min.getSeconds() < 10 ? "0" + r.min.getSeconds() : r.min.getSeconds()) :
					"",
					v = r.max ? "" + (r.max.getSeconds() < 10 ? "0" + r.max.getSeconds() : r.max.getSeconds()) :
					"";
				return r.min ? r.min.setFullYear(2014, 7, 20) : "", r.max ? r.max.setFullYear(2014, 7, 20) :
				"", {
					placeholder: "-",
					allowLeadingZero: !0,
					template: (3 == o.length ? "dd:dd:dd" : 2 == o.length ? "dd:dd" : "dd") + (l ?
						'<span class="mbsc-np-sup">{ampm:--}</span>' : ""),
					leftKey: l ? {
						text: c,
						variable: "ampm:" + c,
						value: "00"
					} : {
						text: ":00",
						value: "00"
					},
					rightKey: l ? {
						text: m,
						variable: "ampm:" + m,
						value: "00"
					} : {
						text: ":30",
						value: "30"
					},
					parseValue: function(e) {
						var t, a, s = e || r.defaultValue,
							n = [];
						if (s) {
							if (s += "", a = s.match(/\d/g))
								for (t = 0; t < a.length; t++) n.push(+a[t]);
							l && n.push("ampm:" + (s.match(new RegExp(r.pmText, "gi")) ? m : c))
						}
						return n
					},
					formatValue: function(e, a) {
						return t(e, a)
					},
					validate: function(a) {
						var n = a.values,
							i = a.variables,
							o = t(n, i),
							c = n.length >= 3 ? new Date(2014, 7, 20, "" + n[0] + (n.length % 2 === 0 ?
								n[1] : ""), "" + n[n.length % 2 === 0 ? 2 : 1] + n[n.length % 2 ===
								0 ? 3 : 2]) : "";
						return {
							disabled: s(n),
							length: d,
							invalid: !((l ? new RegExp(
										"^(0?[1-9]|1[012])(:[0-5]\\d)?(:[0-5][0-9]) (?:" + r
										.amText + "|" + r.pmText + ")$", "i").test(o) :
									/^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(o)) &&
								(r.invalid ? -1 == e._indexOf(r.invalid, c) : 1) && (r.min ? r
									.min <= c : !0) && (r.max ? c <= r.max : !0))
						}
					}
				}
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.numpad,
				n = {
					dateOrder: "mdy",
					dateFormat: "mm/dd/yy",
					delimiter: "/"
				};
			s.date = function(t) {
				function s(e) {
					return e % 4 === 0 && e % 100 !== 0 || e % 400 === 0
				}

				function i(e) {
					var t, a, n, i, r, m = [],
						d = void 0 !== e[o + 3] ? "" + e[o] + e[o + 1] + e[o + 2] + e[o + 3] : "",
						u = void 0 !== e[l + 1] ? "" + e[l] + e[l + 1] : "",
						f = void 0 !== e[c + 1] ? "" + e[c] + e[c + 1] : "",
						y = "" + h.getMaxDayOfMonth(d || 2012, u - 1 || 0),
						w = x === d && +p === +u,
						C = T === d && +b === +u;
					if (h.invalid)
						for (t = 0; t < h.invalid.length; ++t) {
							if (n = h.getYear(h.invalid[t]), i = h.getMonth(h.invalid[t]), r = h.getDay(h
									.invalid[t]), n == +d && i + 1 == +u && (10 > r ? 0 : +("" + r)[0]) == +e[
								c]) {
								m.push(10 > r ? r : +("" + r)[1]);
								break
							}
							if (i + 1 == +u && r == +f && ("" + n).substring(0, 3) == "" + e[o] + e[o + 1] + e[
									o + 2]) {
								m.push(("" + n)[3]);
								break
							}
							if (n == +d && r == +f && (10 > i ? 0 : +("" + (i + 1))[0]) == +e[l]) {
								m.push(10 > i ? i : +("" + (i + 1))[1]);
								break
							}
						}
					if ("31" != f || e.length != l && e.length != l + 1 || (1 != e[l] ? m.push(2, 4, 6, 9, 11) :
							m.push(1)), "30" == f && 0 === e[l] && e.length <= l + 1 && m.push(2), e.length ==
						l) {
						for (t = T === d && 10 > +b ? 1 : 2; 9 >= t; ++t) m.push(t);
						x === d && +p >= 10 && m.push(0)
					}
					if (e.length == l + 1) {
						if (1 == e[l]) {
							for (t = T === d ? +b[1] + 1 : 3; 9 >= t; ++t) m.push(t);
							if (x == d)
								for (t = 0; t < +p[1]; ++t) m.push(t)
						}
						if (0 === e[l] && (m.push(0), T === d || x === d))
							for (t = T === d ? +f > +g ? +b : +b + 1 : 0;
								(x === d ? +v > +f ? +p - 1 : +p - 1 : 9) >= t; ++t) m.push(t)
					}
					if (e.length == c) {
						for (t = C ? (+g > 10 ? +g[0] : 0) + 1 : +y[0] + 1; 9 >= t; ++t) m.push(t);
						if (w)
							for (t = 0; t < (10 > +v ? 0 : v[0]); ++t) m.push(t)
					}
					if (e.length == c + 1) {
						if (e[c] >= 3 || "02" == u)
							for (t = +y[1] + 1; 9 >= t; ++t) m.push(t);
						if (C && +g[0] == e[c])
							for (t = +g[1] + 1; 9 >= t; ++t) m.push(t);
						if (w && v[0] == e[c])
							for (t = 0; t < +v[1]; ++t) m.push(t);
						if (0 === e[c] && (m.push(0), C || w))
							for (t = C ? +g + 1 : 1;
								(w ? +v - 1 : 9) >= t; ++t) m.push(t)
					}
					if (void 0 !== e[o + 2] && "02" == u && "29" == f)
						for (a = +("" + e[o] + e[o + 1] + e[o + 2] + 0); a <= +("" + e[o] + e[o + 1] + e[o +
								2] + 9); ++a) m.push(s(a) ? "" : a % 10);
					if (e.length == o) {
						if (h.min)
							for (t = 0; t < +x[0]; ++t) m.push(t);
						if (h.max)
							for (t = +T[0] + 1; 9 >= t; ++t) m.push(t);
						m.push(0)
					}
					if (h.min || h.max)
						for (a = 1; 4 > a; ++a)
							if (e.length == o + a) {
								if (e[o + a - 1] == +x[a - 1] && (3 == a ? e[o + a - 2] == +x[a - 2] : !0))
									for (t = 0; t < +x[a] + (3 == a && e[l + 1] && +p > +u ? 1 : 0); ++t) m
										.push(t);
								if (e[o + a - 1] == +T[a - 1] && (3 == a ? e[o + a - 2] == +T[a - 2] : !0))
									for (t = +T[a] + (3 == a && +u > +b ? 0 : 1); 9 >= t; ++t) m.push(t)
							} return m
				}

				function r(e) {
					return new Date(+("" + e[o] + e[o + 1] + e[o + 2] + e[o + 3]), +("" + e[l] + e[l + 1]) - 1,
						+("" + e[c] + e[c + 1]))
				}
				var o, l, c, m, d = [],
					u = a.extend({}, t.settings),
					h = a.extend(t.settings, e.util.datetime.defaults, n, u),
					f = h.dateOrder,
					p = h.min ? "" + (h.getMonth(h.min) + 1) : 0,
					b = h.max ? "" + (h.getMonth(h.max) + 1) : 0,
					v = h.min ? "" + h.getDay(h.min) : 0,
					g = h.max ? "" + h.getDay(h.max) : 0,
					x = h.min ? "" + h.getYear(h.min) : 0,
					T = h.max ? "" + h.getYear(h.max) : 0;
				for (f = f.replace(/y+/gi, "yyyy"), f = f.replace(/m+/gi, "mm"), f = f.replace(/d+/gi, "dd"),
					o = f.toUpperCase().indexOf("Y"), l = f.toUpperCase().indexOf("M"), c = f.toUpperCase()
					.indexOf("D"), f = "", d.push({
						val: o,
						n: "yyyy"
					}, {
						val: l,
						n: "mm"
					}, {
						val: c,
						n: "dd"
					}), d.sort(function(e, t) {
						return e.val - t.val
					}), a.each(d, function(e, t) {
						f += t.n
					}), o = f.indexOf("y"), l = f.indexOf("m"), c = f.indexOf("d"), f = "", m = 0; 8 > m; ++m)
					f += "d", (m + 1 == o || m + 1 == l || m + 1 == c) && (f += h.delimiter);
				return t.getVal = function(e) {
					return t._hasValue || e ? r(t.getArrayVal(e)) : null
				}, {
					placeholder: "-",
					fill: "ltr",
					allowLeadingZero: !0,
					template: f,
					parseValue: function(t) {
						var a, s = [],
							n = t || h.defaultValue,
							i = e.util.datetime.parseDate(h.dateFormat, n, h);
						if (n)
							for (a = 0; a < d.length; ++a) s = s.concat(/m/i.test(d[a].n) ? ((h
									.getMonth(i) < 9 ? "0" : "") + (h.getMonth(i) + 1)).split("") :
								/d/i.test(d[a].n) ? ((h.getDay(i) < 10 ? "0" : "") + h.getDay(i))
								.split("") : (h.getYear(i) + "").split(""));
						return s
					},
					formatValue: function(t) {
						return e.util.datetime.formatDate(h.dateFormat, r(t), h)
					},
					validate: function(e) {
						var a = e.values,
							s = r(a);
						return {
							disabled: i(a),
							invalid: !("Invalid Date" != s && (h.min ? h.min <= s : !0) && (h.max ? s <=
								h.max : !0) && (h.invalid ? -1 == t._indexOf(h.invalid, s) : 1))
						}
					}
				}
			}
		}(),
		function(e, a, s) {
			var n = k = t,
				i = n.$,
				r = n.presets.scroller,
				o = n.util,
				l = o.datetime.adjustedDate,
				c = o.jsPrefix,
				m = o.testTouch,
				d = o.getCoord,
				u = {
					controls: ["calendar"],
					firstDay: 0,
					weekDays: "short",
					maxMonthWidth: 170,
					months: 1,
					preMonths: 1,
					highlight: !0,
					outerMonthChange: !0,
					quickNav: !0,
					yearChange: !0,
					todayClass: "mbsc-cal-today",
					btnCalPrevClass: "mbsc-ic mbsc-ic-arrow-left6",
					btnCalNextClass: "mbsc-ic mbsc-ic-arrow-right6",
					dateText: "Date",
					timeText: "Time",
					calendarText: "Calendar",
					todayText: "Today",
					prevMonthText: "Previous Month",
					nextMonthText: "Next Month",
					prevYearText: "Previous Year",
					nextYearText: "Next Year"
				};
			r.calbase = function(e) {
				function t(e) {
					var t;
					Nt = i(this), Ft = !1, "keydown" != e.type ? (At = d(e, "X"), Vt = d(e, "Y"), t = m(e,
						this)) : t = 32 === e.keyCode, Lt || !t || Nt.hasClass("mbsc-fr-btn-d") || (Lt = !0,
						setTimeout(p, 100), "mousedown" != e.type) || i(a).on("mousemove", h).on("mouseup",
						f)
				}

				function h(e) {
					(7 < Math.abs(At - d(e, "X")) || 7 < Math.abs(Vt - d(e, "Y"))) && (Lt = !1, Nt.removeClass(
						"mbsc-fr-btn-a"))
				}

				function f(e) {
					"touchend" == e.type && e.preventDefault(), Nt && !Ft && p(), Lt = !1, "mouseup" == e
						.type && i(a).off("mousemove", h).off("mouseup", f)
				}

				function p() {
					Ft = !0, Nt.hasClass("mbsc-cal-prev-m") ? L() : Nt.hasClass("mbsc-cal-next-m") ? F() : Nt
						.hasClass("mbsc-cal-prev-y") ? H(Nt) : Nt.hasClass("mbsc-cal-next-y") && I(Nt)
				}

				function b(e, t, a) {
					var s, n, r, o, c = {},
						m = la + Ot;
					return e && i.each(e, function(e, i) {
						if (s = i.d || i.start || i, n = s + "", i.start && i.end)
							for (o = new Date(i.start); o <= i.end;) r = l(o.getFullYear(), o
							.getMonth(), o.getDate()), c[r] = c[r] || [], c[r].push(i), o.setDate(o
								.getDate() + 1);
						else if (s.getTime) r = l(s.getFullYear(), s.getMonth(), s.getDate()), c[r] = c[
							r] || [], c[r].push(i);
						else if (n.match(/w/i)) {
							var d = +n.replace("w", ""),
								u = 0,
								h = Xt.getDate(t, a - la - Pt, 1).getDay();
							for (1 < Xt.firstDay - h + 1 && (u = 7), W = 0; 5 * Ht > W; W++) r = Xt
								.getDate(t, a - la - Pt, 7 * W - u - h + 1 + d), c[r] = c[r] || [], c[r]
								.push(i)
						} else if (n = n.split("/"), n[1]) a + m >= 11 && (r = Xt.getDate(t + 1, n[0] -
								1, n[1]), c[r] = c[r] || [], c[r].push(i)), 1 >= a - m && (r = Xt
								.getDate(t - 1, n[0] - 1, n[1]), c[r] = c[r] || [], c[r].push(i)), r =
							Xt.getDate(t, n[0] - 1, n[1]), c[r] = c[r] || [], c[r].push(i);
						else
							for (W = 0; Ht > W; W++) r = Xt.getDate(t, a - la - Pt + W, n[0]), Xt
								.getDay(r) == n[0] && (c[r] = c[r] || [], c[r].push(i))
					}), c
				}

				function v(t, a) {
					St = b(Xt.invalid, t, a), Mt = b(Xt.valid, t, a), e.onGenMonth(t, a)
				}

				function g(e, t, a, s, n, i, r) {
					var o = '<div class="mbsc-cal-h mbsc-cal-sc-c mbsc-cal-' + e + "-c " + (Xt.calendarClass ||
							"") +
						'"><div class="mbsc-cal-sc"><div class="mbsc-cal-sc-p"><div class="mbsc-cal-sc-tbl"><div class="mbsc-cal-sc-row">';
					for (j = 1; t >= j; j++) o = 12 >= j || j > a ? o +
						'<div class="mbsc-cal-sc-m-cell mbsc-cal-sc-cell mbsc-cal-sc-empty"><div class="mbsc-cal-sc-cell-i">&nbsp;</div></div>' :
						o + ('<div tabindex="0" role="button"' + (i ? ' aria-label="' + i[j - 13] + '"' : "") +
							' class="mbsc-fr-btn-e mbsc-fr-btn-nhl mbsc-cal-sc-m-cell mbsc-cal-sc-cell mbsc-cal-' +
							e + '-s" data-val=' + (s + j - 13) +
							'><div class="mbsc-cal-sc-cell-i mbsc-cal-sc-tbl"><div class="mbsc-cal-sc-cell">' +
							(r ? r[j - 13] : s + j - 13 + n) + "</div></div></div>"), t > j && (0 === j % 12 ?
							o += '</div></div></div><div class="mbsc-cal-sc-p" style="' + (ra ? "top" : sa ?
								"right" : "left") + ":" + 100 * Math.round(j / 12) +
							'%"><div class="mbsc-cal-sc-tbl"><div class="mbsc-cal-sc-row">' : 0 === j % 3 && (
								o += '</div><div class="mbsc-cal-sc-row">'));
					return o + "</div></div></div></div></div>"
				}

				function x(t, a) {
					var n, r, o, c, m, d, u, h, f, p, b, v, g, x, T = 1,
						y = 0;
					n = Xt.getDate(t, a, 1);
					var w = Xt.getYear(n),
						C = Xt.getMonth(n),
						M = null !== Xt.defaultValue || e._hasValue ? e.getDate(!0) : null,
						S = Xt.getDate(w, C, 1).getDay(),
						D = '<div class="mbsc-cal-table">',
						k = '<div class="mbsc-cal-week-nr-c">';
					for (1 < Xt.firstDay - S + 1 && (y = 7), x = 0; 42 > x; x++) g = x + Xt.firstDay - y, n = Xt
						.getDate(w, C, g - S + 1), r = n.getFullYear(), o = n.getMonth(), c = n.getDate(), m =
						Xt.getMonth(n), d = Xt.getDay(n), v = Xt.getMaxDayOfMonth(r, o), u = r + "-" + o + "-" +
						c, o = i.extend({
							valid: n < l(vt.getFullYear(), vt.getMonth(), vt.getDate()) || n > gt ? !1 : St[
								n] === s || Mt[n] !== s,
							selected: M && M.getFullYear() === r && M.getMonth() === o && M.getDate() === c
						}, e.getDayProps(n, M)), h = o.valid, f = o.selected, r = o.cssClass, p = new Date(n)
						.setHours(12, 0, 0, 0) === (new Date).setHours(12, 0, 0, 0), b = m !== C, Ut[u] = o,
						0 === x % 7 && (D += (x ? "</div>" : "") + '<div class="mbsc-cal-row' + (Xt.highlight &&
							M && M - n >= 0 && 6048e5 > M - n ? " mbsc-cal-week-hl" : "") + '">'), Qt && 1 == n
						.getDay() && ("month" == Qt && b && T > 1 ? T = 1 == c ? 1 : 2 : "year" == Qt && (T = Xt
								.getWeekNumber(n)), k +=
							'<div class="mbsc-cal-week-nr"><div class="mbsc-cal-week-nr-i">' + T +
							"</div></div>", T++), D += '<div role="button" tabindex="-1" aria-label="' + (p ? Xt
							.todayText + ", " : "") + Xt.dayNames[n.getDay()] + ", " + Xt.monthNames[m] + " " +
						d + " " + (o.ariaLabel ? ", " + o.ariaLabel : "") + '"' + (b && !Yt ?
							' aria-hidden="true"' : "") + (f ? ' aria-selected="true"' : "") + (h ? "" :
							' aria-disabled="true"') + ' data-day="' + g % 7 + '" data-full="' + u +
						'"class="mbsc-cal-day ' + (Xt.dayClass || "") + (f ? " mbsc-cal-day-sel" : "") + (p ?
							" " + Xt.todayClass : "") + (r ? " " + r : "") + (1 == d ? " mbsc-cal-day-first" :
							"") + (d == v ? " mbsc-cal-day-last" : "") + (b ? " mbsc-cal-day-diff" : "") + (h ?
							" mbsc-cal-day-v mbsc-fr-btn-e mbsc-fr-btn-nhl" : " mbsc-cal-day-inv") +
						'"><div class="mbsc-cal-day-i ' + (f ? fa : "") + " " + (Xt.innerDayClass || "") +
						'"><div class="mbsc-cal-day-fg">' + d + "</div>" + (o.markup || "") +
						'<div class="mbsc-cal-day-frame"></div></div></div>';
					return D + ("</div></div>" + k + "</div>")
				}

				function T(e, t, a) {
					var s = Xt.getDate(e, t, 1),
						n = Xt.getYear(s),
						s = Xt.getMonth(s),
						r = n + ha;
					if (oa) {
						if (jt && jt.removeClass("mbsc-cal-sc-sel").removeAttr("aria-selected").find(
								".mbsc-cal-sc-cell-i").removeClass(fa), Wt && Wt.removeClass("mbsc-cal-sc-sel")
							.removeAttr("aria-selected").find(".mbsc-cal-sc-cell-i").removeClass(fa), jt = i(
								'.mbsc-cal-year-s[data-val="' + n + '"]', R).addClass("mbsc-cal-sc-sel").attr(
								"aria-selected", "true"), Wt = i('.mbsc-cal-month-s[data-val="' + s + '"]', R)
							.addClass("mbsc-cal-sc-sel").attr("aria-selected", "true"), jt.find(
								".mbsc-cal-sc-cell-i").addClass(fa), Wt.find(".mbsc-cal-sc-cell-i").addClass(
							fa), zt && zt.scroll(jt, a), i(".mbsc-cal-month-s", R).removeClass("mbsc-fr-btn-d"),
							n === dt)
							for (j = 0; ht > j; j++) i('.mbsc-cal-month-s[data-val="' + j + '"]', R).addClass(
								"mbsc-fr-btn-d");
						if (n === ut)
							for (j = ft + 1; 12 >= j; j++) i('.mbsc-cal-month-s[data-val="' + j + '"]', R)
								.addClass("mbsc-fr-btn-d")
					}
					for (1 == ct.length && ct.attr("aria-label", n).html(r), j = 0; It > j; ++j) s = Xt.getDate(
							e, t - Pt + j, 1), n = Xt.getYear(s), s = Xt.getMonth(s), r = n + ha, i(ot[j]).attr(
							"aria-label", Xt.monthNames[s] + (ca ? "" : " " + n)).html((!ca && lt > mt ? r +
							" " : "") + nt[s] + (!ca && mt > lt ? " " + r : "")), 1 < ct.length && i(ct[j])
						.html(r);
					Xt.getDate(e, t - Pt - 1, 1) < pt ? w(i(".mbsc-cal-prev-m", R)) : y(i(".mbsc-cal-prev-m",
							R)), Xt.getDate(e, t + It - Pt, 1) > bt ? w(i(".mbsc-cal-next-m", R)) : y(i(
							".mbsc-cal-next-m", R)), Xt.getDate(e, t, 1).getFullYear() <= pt.getFullYear() ? w(
							i(".mbsc-cal-prev-y", R)) : y(i(".mbsc-cal-prev-y", R)), Xt.getDate(e, t, 1)
						.getFullYear() >= bt.getFullYear() ? w(i(".mbsc-cal-next-y", R)) : y(i(
							".mbsc-cal-next-y", R))
				}

				function y(e) {
					e.removeClass(va).find(".mbsc-cal-btn-txt").removeAttr("aria-disabled")
				}

				function w(e) {
					e.addClass(va).find(".mbsc-cal-btn-txt").attr("aria-disabled", "true")
				}

				function C(t, a) {
					if (Z && ("calendar" === kt || a)) {
						var s, n, r = Xt.getDate(Tt, yt, 1),
							o = Math.abs(12 * (Xt.getYear(t) - Xt.getYear(r)) + Xt.getMonth(t) - Xt.getMonth(
							r));
						e.needsSlide && o && (Tt = Xt.getYear(t), yt = Xt.getMonth(t), t > r ? (n = o > la -
							Pt + It - 1, yt -= n ? 0 : o - la, s = "next") : r > t && (n = o > la + Pt,
							yt += n ? 0 : o - la, s = "prev"), _(Tt, yt, s, Math.min(o, la), n, !0)), a || (
							xt = t, e.trigger("onDayHighlight", {
								date: t
							}), Xt.highlight && (i(".mbsc-cal-day-sel .mbsc-cal-day-i", q).removeClass(fa),
								i(".mbsc-cal-day-sel", q).removeClass("mbsc-cal-day-sel").removeAttr(
									"aria-selected"), i(".mbsc-cal-week-hl", q).removeClass(
									"mbsc-cal-week-hl"), (null !== Xt.defaultValue || e._hasValue) && i(
									'.mbsc-cal-day[data-full="' + t.getFullYear() + "-" + t.getMonth() +
									"-" + t.getDate() + '"]', q).addClass("mbsc-cal-day-sel").attr(
									"aria-selected", "true").find(".mbsc-cal-day-i").addClass(fa).closest(
									".mbsc-cal-row").addClass("mbsc-cal-week-hl"))), e.needsSlide = !0
					}
				}

				function M(t, a, n) {
					for (n || e.trigger("onMonthLoading", {
							year: t,
							month: a
						}), v(t, a), j = 0; Ht > j; j++) Rt[j].html(x(t, a - Pt - la + j));
					D(), Et = s, e.trigger("onMonthLoaded", {
						year: t,
						month: a
					})
				}

				function S(e, t, a) {
					var s = la,
						n = la;
					if (a) {
						for (; n && Xt.getDate(e, t + s + It - Pt - 1, 1) > bt;) n--;
						for (; s && Xt.getDate(e, t - n - Pt, 1) < pt;) s--
					}
					i.extend(it.settings, {
						contSize: It * K,
						snap: K,
						minScroll: G - (sa ? s : n) * K,
						maxScroll: G + (sa ? n : s) * K
					}), it.refresh()
				}

				function D() {
					Qt && tt.html(i(".mbsc-cal-week-nr-c", Rt[la]).html()), i(".mbsc-cal-slide-a .mbsc-cal-day",
						U).attr("tabindex", 0)
				}

				function _(t, a, n, r, o, l, c) {
					if (t && Bt.push({
							y: t,
							m: a,
							dir: n,
							slideNr: r,
							load: o,
							active: l,
							callback: c
						}), !_t) {
						var m = Bt.shift(),
							t = m.y,
							a = m.m,
							n = "next" === m.dir,
							r = m.slideNr,
							o = m.load,
							l = m.active,
							c = m.callback || Kt,
							m = Xt.getDate(t, a, 1),
							t = Xt.getYear(m),
							a = Xt.getMonth(m);
						if (_t = !0, e.changing = !0, e.trigger("onMonthChange", {
								year: t,
								month: a
							}), e.trigger("onMonthLoading", {
								year: t,
								month: a
							}), v(t, a), o)
							for (j = 0; It > j; j++) Rt[n ? Ht - It + j : j].html(x(t, a - Pt + j));
						l && Jt.addClass("mbsc-cal-slide-a"), setTimeout(function() {
							e.ariaMessage(Xt.monthNames[a] + " " + t), T(t, a, 200), G = n ? G - K * r *
								na : G + K * r * na, it.scroll(G, l ? 200 : 0, !1, function() {
									var l;
									if (Rt.length) {
										if (Jt.removeClass("mbsc-cal-slide-a").attr("aria-hidden",
												"true"), n)
											for (l = Rt.splice(0, r), j = 0; r > j; j++) Rt.push(l[
												j]), V(Rt[Rt.length - 1], +Rt[Rt.length - 2]
												.attr("data-curr") + 100 * na);
										else
											for (l = Rt.splice(Ht - r, r), j = r - 1; j >= 0; j--)
												Rt.unshift(l[j]), V(Rt[0], +Rt[1].attr(
													"data-curr") - 100 * na);
										for (j = 0; r > j; j++) Rt[n ? Ht - r + j : j].html(x(t, a -
											Pt - la + j + (n ? Ht - r : 0))), o && Rt[n ? j :
											Ht - r + j].html(x(t, a - Pt - la + j + (n ? 0 :
											Ht - r)));
										for (j = 0; It > j; j++) Rt[la + j].addClass(
											"mbsc-cal-slide-a").removeAttr("aria-hidden");
										S(t, a, !0), _t = !1
									}
									Bt.length ? setTimeout(function() {
										_()
									}, 10) : (Tt = t, yt = a, e.changing = !1, i(
											".mbsc-cal-day", U).attr("tabindex", -1), D(),
										Et !== s ? M(t, a, Et) : e.trigger("onMonthLoaded", {
											year: t,
											month: a
										}), c())
								})
						}, 10)
					}
				}

				function A() {
					var t = i(this),
						a = e.live,
						s = e.getDate(!0),
						n = t.attr("data-full"),
						r = n.split("-"),
						r = l(r[0], r[1], r[2]),
						s = l(r.getFullYear(), r.getMonth(), r.getDate(), s.getHours(), s.getMinutes(), s
							.getSeconds()),
						o = t.hasClass("mbsc-cal-day-sel");
					!Yt && t.hasClass("mbsc-cal-day-diff") || !1 === e.trigger("onDayChange", i.extend(Ut[n], {
						date: s,
						target: this,
						selected: o
					})) || (e.needsSlide = !1, X = !0, e.setDate(s, a, .2, !a, !0), Xt.outerMonthChange && (
						Lt = !0, r < Xt.getDate(Tt, yt - Pt, 1) ? L() : r > Xt.getDate(Tt, yt - Pt + It,
							0) && F(), Lt = !1), e.live && e.trigger("onSet", {
						valueText: e._value
					}))
				}

				function V(e, t) {
					e.attr("data-curr", t), e[0].style[c + "Transform"] = "translate3d(" + (ra ? "0," + t +
						"%," : t + "%,0,") + "0)"
				}

				function N(t) {
					e.isVisible() && Z && (e.changing ? Et = t : M(Tt, yt, t))
				}

				function F() {
					Lt && Xt.getDate(Tt, yt + It - Pt, 1) <= bt && _(Tt, ++yt, "next", 1, !1, !0, F)
				}

				function L() {
					Lt && Xt.getDate(Tt, yt - Pt - 1, 1) >= pt && _(Tt, --yt, "prev", 1, !1, !0, L)
				}

				function I(e) {
					Lt && Xt.getDate(Tt, yt, 1) <= Xt.getDate(Xt.getYear(bt) - 1, Xt.getMonth(bt) - Ot, 1) && k
						.running ? _(++Tt, yt, "next", la, !0, !0, function() {
							I(e)
						}) : Lt && !e.hasClass("mbsc-fr-btn-d") && _(Xt.getYear(bt), Xt.getMonth(bt) - Ot,
							"next", la, !0, !0)
				}

				function H(e) {
					Lt && Xt.getDate(Tt, yt, 1) >= Xt.getDate(Xt.getYear(pt) + 1, Xt.getMonth(pt) + Pt, 1) && k
						.running ? _(--Tt, yt, "prev", la, !0, !0, function() {
							H(e)
						}) : Lt && !e.hasClass("mbsc-fr-btn-d") && _(Xt.getYear(pt), Xt.getMonth(pt) + Pt,
							"prev", la, !0, !0)
				}

				function O(t, a) {
					t.hasClass("mbsc-cal-v") || (t.addClass("mbsc-cal-v" + (a ? "" : " mbsc-cal-p-in"))
						.removeClass("mbsc-cal-p-out mbsc-cal-h"), e.trigger("onSelectShow"))
				}

				function P(e, t) {
					e.hasClass("mbsc-cal-v") && e.removeClass("mbsc-cal-v mbsc-cal-p-in").addClass(
						"mbsc-cal-h" + (t ? "" : " mbsc-cal-p-out"))
				}

				function E(e, t) {
					(t || e).hasClass("mbsc-cal-v") ? P(e) : O(e)
				}

				function Y() {
					i(this).removeClass("mbsc-cal-p-out mbsc-cal-p-in")
				}
				var z, j, W, $, J, R, B, q, U, K, G, X, Z, Q, et, tt, at, st, nt, it, rt, ot, lt, ct, mt, dt,
					ut, ht, ft, pt, bt, vt, gt, xt, Tt, yt, wt, Ct, Mt, St, Dt, kt, _t, At, Vt, Nt, Ft, Lt, It,
					Ht, Ot, Pt, Et, Yt, zt, jt, Wt, $t = this,
					Jt = [],
					Rt = [],
					Bt = [],
					qt = {},
					Ut = {},
					Kt = function() {},
					Gt = i.extend({}, e.settings),
					Xt = i.extend(e.settings, u, Gt),
					Zt = "full" == Xt.weekDays ? "" : "min" == Xt.weekDays ? "Min" : "Short",
					Qt = Xt.weekCounter,
					ea = Xt.layout || (/top|bottom/.test(Xt.display) ? "liquid" : ""),
					ta = "liquid" == ea && "bubble" !== Xt.display,
					aa = "center" == Xt.display,
					sa = Xt.rtl,
					na = sa ? -1 : 1,
					ia = ta ? null : Xt.calendarWidth,
					ra = "vertical" == Xt.calendarScroll,
					oa = Xt.quickNav,
					la = Xt.preMonths,
					ca = Xt.yearChange,
					ma = Xt.controls.join(","),
					da = (!0 === Xt.tabs || !1 !== Xt.tabs && ta) && 1 < Xt.controls.length,
					ua = !da && Xt.tabs === s && !ta && 1 < Xt.controls.length,
					ha = Xt.yearSuffix || "",
					fa = Xt.activeClass || "",
					pa = "mbsc-cal-tab-sel " + (Xt.activeTabClass || ""),
					ba = Xt.activeTabInnerClass || "",
					va = "mbsc-fr-btn-d " + (Xt.disabledClass || ""),
					ga = "",
					xa = "";
				return ma.match(/calendar/) ? Z = !0 : oa = !1, ma.match(/date/) && (qt.date = 1), ma.match(
						/time/) && (qt.time = 1), Z && qt.date && (da = !0, ua = !1), Xt.layout = ea, Xt
					.preset = (qt.date || Z ? "date" : "") + (qt.time ? "time" : ""), "inline" == Xt.display &&
					i(this).closest('[data-role="page"]').on("pageshow", function() {
						e.position()
					}), e.changing = !1, e.needsSlide = !0, e.getDayProps = Kt, e.onGenMonth = Kt, e
					.prepareObj = b, e.refresh = function() {
						N(!1)
					}, e.redraw = function() {
						N(!0)
					}, e.navigate = function(t, a) {
						var s, n, i = e.isVisible();
						a && i ? C(t, !0) : (s = Xt.getYear(t), n = Xt.getMonth(t), !i || s == Tt && n == yt ||
							(e.trigger("onMonthChange", {
								year: s,
								month: n
							}), T(s, n), M(s, n), S(t.getFullYear(), t.getMonth(), !0)), Tt = s, yt = n)
					}, e.showMonthView = function() {
						oa && !st && (P(xa, !0), P(ga, !0), O(at, !0), st = !0)
					}, e.changeTab = function(t) {
						e._isVisible && qt[t] && kt != t && (kt = t, i(".mbsc-cal-pnl", R).removeClass(
								"mbsc-cal-p-in").addClass("mbsc-cal-pnl-h"), i(".mbsc-cal-tab", R)
							.removeClass(pa).removeAttr("aria-selected").find(".mbsc-cal-tab-i")
							.removeClass(ba), i('.mbsc-cal-tab[data-control="' + t + '"]', R).addClass(pa)
							.attr("aria-selected", "true").find(".mbsc-cal-tab-i").addClass(ba), qt[kt]
							.removeClass("mbsc-cal-pnl-h").addClass("mbsc-cal-p-in"), "calendar" == kt && (
								z = e.getDate(!0), (z.getFullYear() !== xt.getFullYear() || z.getMonth() !==
									xt.getMonth() || z.getDate() !== xt.getDate()) && C(z)), e
							.showMonthView(), e.trigger("onTabChange", {
								tab: kt
							}))
					}, $ = r.datetime.call(this, e), lt = Xt.dateFormat.search(/m/i), mt = Xt.dateFormat.search(
						/y/i), i.extend($, {
						ariaMessage: Xt.calendarText,
						onMarkupReady: function(a) {
							var r, c = "";
							if (R = i(a.target), B = "inline" == Xt.display ? i(this).is("div") ? i(
									this) : i(this).parent() : e._window, xt = e.getDate(!0), Tt || (
									Tt = Xt.getYear(xt), yt = Xt.getMonth(xt)), G = 0, et = !0, _t = !1,
								nt = Xt.monthNames, kt = "calendar", Xt.min ? (pt = l(Xt.min
									.getFullYear(), Xt.min.getMonth(), 1), vt = Xt.min) : vt = pt = l(Xt
									.startYear, 0, 1), Xt.max ? (bt = l(Xt.max.getFullYear(), Xt.max
									.getMonth(), 1), gt = Xt.max) : gt = bt = l(Xt.endYear, 11, 31, 23,
									59, 59), R.addClass("mbsc-calendar"), J = i(".mbsc-fr-popup", R),
								Dt = i(".mbsc-fr-c", R), qt.date ? qt.date = i(".mbsc-sc-whl-gr-c", R)
								.eq(0) : Z && i(".mbsc-sc-whl-gr-c", R).eq(0).addClass("mbsc-cal-hdn"),
								qt.time && (qt.time = i(".mbsc-sc-whl-gr-c", R).eq(1)), Z) {
								for (It = "auto" == Xt.months ? Math.max(1, Math.min(3, Math.floor((
										ia || B[0].innerWidth || B.innerWidth()) / 280))) : Xt.months,
									Ht = It + 2 * la, Ot = Math.floor(It / 2), Pt = Math.round(It / 2) -
									1, Yt = Xt.showOuterDays === s ? 2 > It : Xt.showOuterDays, ra =
									ra && 2 > It, a = '<div class="mbsc-cal-btnw"><div class="' + (sa ?
										"mbsc-cal-next-m" : "mbsc-cal-prev-m") +
									' mbsc-cal-prev mbsc-cal-btn mbsc-fr-btn mbsc-fr-btn-e"><div role="button" tabindex="0" class="mbsc-cal-btn-txt ' +
									(Xt.btnCalPrevClass || "") + '" aria-label="' + Xt.prevMonthText +
									'"></div></div>', j = 0; It > j; ++j) a +=
									'<div class="mbsc-cal-btnw-m" style="width: ' + 100 / It +
									'%"><span role="button" class="mbsc-cal-month"></span></div>';
								for (a += '<div class="' + (sa ? "mbsc-cal-prev-m" :
									"mbsc-cal-next-m") +
									' mbsc-cal-next mbsc-cal-btn mbsc-fr-btn mbsc-fr-btn-e"><div role="button" tabindex="0" class="mbsc-cal-btn-txt ' +
									(Xt.btnCalNextClass || "") + '" aria-label="' + Xt.nextMonthText +
									'"></div></div></div>', ca && (c =
										'<div class="mbsc-cal-btnw"><div class="' + (sa ?
											"mbsc-cal-next-y" : "mbsc-cal-prev-y") +
										' mbsc-cal-prev mbsc-cal-btn mbsc-fr-btn mbsc-fr-btn-e"><div role="button" tabindex="0" class="mbsc-cal-btn-txt ' +
										(Xt.btnCalPrevClass || "") + '" aria-label="' + Xt
										.prevYearText +
										'"></div></div><span role="button" class="mbsc-cal-year"></span><div class="' +
										(sa ? "mbsc-cal-prev-y" : "mbsc-cal-next-y") +
										' mbsc-cal-next mbsc-cal-btn mbsc-fr-btn mbsc-fr-btn-e"><div role="button" tabindex="0" class="mbsc-cal-btn-txt ' +
										(Xt.btnCalNextClass || "") + '" aria-label="' + Xt
										.nextYearText + '"></div></div></div>'), oa && (dt = Xt.getYear(
											pt), ut = Xt.getYear(bt), ht = Xt.getMonth(pt), ft = Xt
										.getMonth(bt), Ct = Math.ceil((ut - dt + 1) / 12) + 2, ga = g(
											"month", 36, 24, 0, "", Xt.monthNames, Xt.monthNamesShort),
										xa = g("year", 12 * Ct, ut - dt + 13, dt, ha)), Q =
									'<div class="mbsc-w-p mbsc-cal-c"><div class="mbsc-cal mbsc-cal-hl-now' +
									(It > 1 ? " mbsc-cal-multi " : "") + (Qt ? " mbsc-cal-weeks " :
									"") + (ra ? " mbsc-cal-vertical" : "") + (Yt ? "" :
										" mbsc-cal-hide-diff ") + (Xt.calendarClass || "") +
									'"><div class="mbsc-cal-header"><div class="mbsc-cal-btnc ' + (ca ?
										"mbsc-cal-btnc-ym" : "mbsc-cal-btnc-m") + '">' + (lt > mt ||
										It > 1 ? c + a : a + c) +
									'</div></div><div class="mbsc-cal-body"><div class="mbsc-cal-m-c mbsc-cal-v"><div class="mbsc-cal-days-c">',
									W = 0; It > W; ++W) {
									for (Q +=
										'<div aria-hidden="true" class="mbsc-cal-days" style="width: ' +
										100 / It + '%"><table cellpadding="0" cellspacing="0"><tr>', j =
										0; 7 > j; j++) Q += "<th>" + Xt["dayNames" + Zt][(j + Xt
										.firstDay) % 7] + "</th>";
									Q += "</tr></table></div>"
								}
								for (Q += '</div><div class="mbsc-cal-week-nrs-c ' + (Xt.weekNrClass ||
										"") +
									'"><div class="mbsc-cal-week-nrs"></div></div><div class="mbsc-cal-anim-c ' +
									(Xt.calendarClass || "") + '"><div class="mbsc-cal-anim">', j =
									0; It + 2 * la > j; j++) Q +=
									'<div class="mbsc-cal-slide" aria-hidden="true"></div>';
								Q += "</div></div></div>" + ga + xa + "</div></div></div>", qt
									.calendar = i(Q)
							}
							if (i.each(Xt.controls, function(e, t) {
									qt[t] = i('<div class="mbsc-cal-pnl" id="' + ($t.id +
											"_dw_pnl_" + e) + '"></div>').append(i(
											'<div class="mbsc-cal-pnl-i"></div>').append(qt[t]))
										.appendTo(Dt)
								}), r = '<div class="mbsc-cal-tabs"><ul role="tablist">', i.each(Xt
									.controls,
									function(e, t) {
										qt[t] && (r += '<li role="tab" aria-controls="' + ($t.id +
												"_dw_pnl_" + e) + '" class="mbsc-cal-tab ' + (e ?
												"" : pa) + '" data-control="' + t +
											'"><a href="#" class="mbsc-fr-btn-e mbsc-fr-btn-nhl mbsc-cal-tab-i ' +
											(e ? "" : ba) + '">' + Xt[t + "Text"] + "</a></li>")
									}), r += "</ul></div>", Dt.before(r), q = i(".mbsc-cal-anim-c", R),
								U = i(".mbsc-cal-anim", q), tt = i(".mbsc-cal-week-nrs", R), Z) {
								for (st = !0, Jt = i(".mbsc-cal-slide", U).each(function(e, t) {
										Rt.push(i(t))
									}), Jt.slice(la, la + It).addClass("mbsc-cal-slide-a").removeAttr(
										"aria-hidden"), j = 0; Ht > j; j++) V(Rt[j], 100 * (j - la) *
									na);
								M(Tt, yt), it = new n.classes.ScrollView(q[0], {
									axis: ra ? "Y" : "X",
									easing: "",
									contSize: 0,
									snap: 1,
									maxSnapScroll: la,
									moveElement: U,
									mousewheel: Xt.mousewheel,
									time: 200,
									lock: !0,
									stopProp: !1,
									onAnimationEnd: function(e) {
										(e = Math.round(((ra ? e.posY : e.posX) - G) / K) *
											na) && _(Tt, yt - e, e > 0 ? "prev" : "next",
											e > 0 ? e : -e)
									}
								})
							}
							ot = i(".mbsc-cal-month", R), ct = i(".mbsc-cal-year", R), at = i(
								".mbsc-cal-m-c", R), oa && (at.on("webkitAnimationEnd animationend",
									Y), ga = i(".mbsc-cal-month-c", R).on(
									"webkitAnimationEnd animationend", Y), xa = i(
									".mbsc-cal-year-c", R).on("webkitAnimationEnd animationend", Y),
								i(".mbsc-cal-sc-p", R), wt = {
									axis: ra ? "Y" : "X",
									contSize: 0,
									snap: 1,
									maxSnapScroll: 1,
									rtl: Xt.rtl,
									mousewheel: Xt.mousewheel,
									time: 200
								}, zt = new n.classes.ScrollView(xa[0], wt), rt = new n.classes
								.ScrollView(ga[0], wt)), ta ? R.addClass("mbsc-cal-liq") : i(
								".mbsc-cal", R).width(ia || 280 * It), Xt.calendarHeight && i(
								".mbsc-cal-anim-c", R).height(Xt.calendarHeight), e.tap(q, function(
								e) {
								e = i(e.target), _t || it.scrolled || Xt.readonly === !0 || (e =
									e.closest(".mbsc-cal-day", this), e.hasClass(
										"mbsc-cal-day-v") && A.call(e[0]))
							}), i(".mbsc-cal-btn", R).on("touchstart mousedown keydown", t).on(
								"touchmove", h).on("touchend touchcancel keyup", f), i(
								".mbsc-cal-tab", R).on("touchstart click", function(t) {
								m(t, this) && e.changeTab(i(this).attr("data-control"))
							}), oa && (e.tap(i(".mbsc-cal-month", R), function() {
								xa.hasClass("mbsc-cal-v") || (E(at), st = at.hasClass(
									"mbsc-cal-v")), E(ga), P(xa)
							}), e.tap(i(".mbsc-cal-year", R), function() {
								xa.hasClass("mbsc-cal-v") || zt.scroll(jt), ga.hasClass(
									"mbsc-cal-v") || (E(at), st = at.hasClass(
									"mbsc-cal-v")), E(xa), P(ga)
							}), e.tap(i(".mbsc-cal-month-s", R), function() {
								!rt.scrolled && !i(this).hasClass("mbsc-fr-btn-d") && e
									.navigate(Xt.getDate(Tt, i(this).attr("data-val"), 1))
							}), e.tap(i(".mbsc-cal-year-s", R), function() {
								zt.scrolled || (z = Xt.getDate(i(this).attr("data-val"), yt,
										1), e.navigate(new Date(o.constrain(z, pt,
									bt))))
							}), e.tap(xa, function() {
								zt.scrolled || (P(xa), O(at), st = !0)
							}), e.tap(ga, function() {
								rt.scrolled || (P(ga), O(at), st = !0)
							}))
						},
						onShow: function() {
							Z && T(Tt, yt)
						},
						onPosition: function(t) {
							var a, s, n, r = 0,
								o = 0,
								l = 0,
								c = t.windowHeight;
							if (ta && (aa && q.height(""), Dt.height(""), U.width("")), K && (n = K),
								Z && (K = Math.round(Math.round(q[0][ra ? "offsetHeight" :
									"offsetWidth"]) / It)), K && (R.removeClass(
									"mbsc-cal-m mbsc-cal-l"), K > 1024 ? R.addClass("mbsc-cal-l") : K >
									640 && R.addClass("mbsc-cal-m")), (da && (et || ta) || ua) && (i(
									".mbsc-cal-pnl", R).removeClass("mbsc-cal-pnl-h"), i.each(qt,
									function(e, t) {
										a = t[0].offsetWidth, r = Math.max(r, a), o = Math.max(o, t[
											0].offsetHeight), l += a
									}), da || ua && l > (B[0].innerWidth || B.innerWidth()) ? (s = !
									0, kt = i(".mbsc-cal-tabs .mbsc-cal-tab-sel", R).attr(
										"data-control"), J.addClass("mbsc-cal-tabbed")) : (kt =
									"calendar", o = r = "", J.removeClass("mbsc-cal-tabbed"), Dt
									.css({
										width: "",
										height: ""
									}))), ta && aa && Z && (e._isFullScreen = !0, s && Dt.height(qt
										.calendar[0].offsetHeight), t = J[0].offsetHeight, c >= t && q
									.height(c - t + q[0].offsetHeight), o = Math.max(o, qt.calendar[0]
										.offsetHeight)), s && (Dt.css({
									width: ta ? "" : r,
									height: o
								}), Z && (K = Math.round(Math.round(q[0][ra ? "offsetHeight" :
									"offsetWidth"
								]) / It))), K) {
								if (U[ra ? "height" : "width"](K), K !== n) {
									if (ca)
										for (nt = Xt.maxMonthWidth > i(".mbsc-cal-btnw-m", R).width() ?
											Xt.monthNamesShort : Xt.monthNames, j = 0; It > j; ++j) i(
											ot[j]).text(nt[Xt.getMonth(Xt.getDate(Tt, yt - Pt + j,
											1))]);
									oa && (t = xa[0][ra ? "offsetHeight" : "offsetWidth"], i.extend(zt
											.settings, {
												contSize: t,
												snap: t,
												minScroll: (2 - Ct) * t,
												maxScroll: -t
											}), i.extend(rt.settings, {
											contSize: t,
											snap: t,
											minScroll: -t,
											maxScroll: -t
										}), zt.refresh(), rt.refresh(), xa.hasClass("mbsc-cal-v") &&
										zt.scroll(jt)), ta && !et && n && (t = G / n, G = t * K), S(
										Tt, yt, !n)
								}
							} else K = n;
							s && (i(".mbsc-cal-pnl", R).addClass("mbsc-cal-pnl-h"), qt[kt].removeClass(
								"mbsc-cal-pnl-h")), e.trigger("onCalResize"), et = !1
						},
						onHide: function() {
							Bt = [], Rt = [], yt = Tt = kt = null, _t = !0, K = 0, it && it.destroy(),
								oa && zt && rt && (zt.destroy(), rt.destroy())
						},
						onValidated: function(t) {
							var a, s, n;
							if (s = e.getDate(!0), X) a = "calendar";
							else
								for (n in e.order) n && e.order[n] === t && (a = /[mdy]/.test(n) ?
									"date" : "time");
							e.trigger("onSetDate", {
								date: s,
								control: a
							}), C(s), X = !1
						}
					}), $
			}
		}(window, document),
		function(e, a, s) {
			var n = t,
				i = n.$,
				r = i.extend,
				o = n.util,
				l = o.datetime,
				c = l.adjustedDate,
				m = n.presets.scroller,
				d = {};
			n.presetShort("calendar"), m.calendar = function(t) {
				function a(t) {
					if (t) {
						if (y[t]) return y[t];
						var a = i('<div style="background-color:' + t + ';"></div>').appendTo("body"),
							s = e.getComputedStyle ? getComputedStyle(a[0]) : a[0].style,
							n = s.backgroundColor.replace(/rgb|rgba|\(|\)|\s/g, "").split(","),
							r = .299 * n[0] + .587 * n[1] + .114 * n[2],
							o = r > 130 ? "#000" : "#fff";
						return a.remove(), y[t] = o, o
					}
				}

				function n(e) {
					return c(e.getFullYear(), e.getMonth(), e.getDate())
				}

				function u(e) {
					if (_ = {}, e && e.length)
						for (v = 0; v < e.length; v++) _[n(e[v])] = e[v]
				}

				function h() {
					t.refresh()
				}
				var f, p, b, v, g, x, T, y = {},
					w = r({}, t.settings),
					C = r(t.settings, d, w),
					M = C.activeClass || "",
					S = "multiple" == C.select || C.select > 1 || "week" == C.selectType,
					D = o.isNumeric(C.select) ? C.select : 1 / 0,
					k = !!C.events,
					_ = {};
				if (T = m.calbase.call(this, t), f = r({}, T), b = C.firstSelectDay === s ? C.firstDay : C
					.firstSelectDay, S && C.defaultValue && C.defaultValue.length)
					for (v = 0; v < C.defaultValue.length; v++) _[n(C.defaultValue[v])] = C.defaultValue[v];
				return t.onGenMonth = function(e, a) {
					g = t.prepareObj(C.events || C.marked, e, a)
				}, t.getDayProps = function(e) {
					var t, n = S ? _[e] !== s : s,
						r = g[e] ? g[e] : !1,
						o = r && r[0] && r[0].text,
						l = r && r[0] && r[0].color,
						c = k && o ? a(l) : "",
						m = "",
						d = "";
					if (r) {
						for (t = 0; t < r.length; t++) r[t].icon && (m += '<span class="mbsc-ic mbsc-ic-' +
							r[t].icon + '"' + (r[t].text ? "" : r[t].color ? ' style="color:' + r[t]
								.color + ';"' : "") + "></span>\n");
						for (d = '<div class="mbsc-cal-day-m"><div class="mbsc-cal-day-m-t">', t = 0; t < r
							.length; t++) d += '<div class="mbsc-cal-day-m-c"' + (r[t].color ?
							' style="background:' + r[t].color + ';"' : "") + "></div>";
						d += "</div></div>"
					}
					return {
						marked: r,
						selected: n,
						cssClass: r ? "mbsc-cal-day-marked" : "",
						ariaLabel: k ? o : "",
						markup: k && o ?
							'<div class="mbsc-cal-day-txt-c"><div class="mbsc-cal-day-txt" title="' + i(
								"<div>" + o + "</div>").text() + '"' + (l ? ' style="background:' + l +
								";color:" + c + ';text-shadow:none;"' : "") + ">" + m + o + "</div></div>" :
							k && m ? '<div class="mbsc-cal-day-ic-c">' + m + "</div>" : r ? d : ""
					}
				}, t.addValue = function(e) {
					_[n(e)] = e, h()
				}, t.removeValue = function(e) {
					delete _[n(e)], h()
				}, t.setVal = function(e, a, s, n, i) {
					S && (u(e), e = e ? e[0] : null), t._setVal(e, a, s, n, i), h()
				}, t.getVal = function(e) {
					return S ? o.objectToArray(_) : t.getDate(e)
				}, r(T, {
					highlight: !S,
					outerMonthChange: !S,
					parseValue: function(e) {
						var a, s;
						if (S && e && "string" == typeof e) {
							for (_ = {}, e = e.split(","), a = 0; a < e.length; a++) s = l
								.parseDate(t.format, e[a].replace(/^\s+|\s+$/g, ""), C), _[n(s)] =
								s;
							e = e[0]
						}
						return S && C.defaultValue && C.defaultValue.length && (C.defaultValue = C
							.defaultValue[0]), f.parseValue.call(this, e)
					},
					formatValue: function(e) {
						var a, s = [];
						if (S) {
							for (a in _) s.push(l.formatDate(t.format, _[a], C));
							return s.join(", ")
						}
						return f.formatValue.call(this, e)
					},
					onClear: function() {
						S && (_ = {}, t.refresh())
					},
					onBeforeShow: function() {
						C.setOnDayTap !== s || C.buttons && C.buttons.length || (C.setOnDayTap = !
							0), C.setOnDayTap && "inline" != C.display && (C.outerMonthChange = !1),
							C.counter && S && (C.headerText = function() {
								var e = 0,
									t = "week" == C.selectType ? 7 : 1;
								return i.each(_, function() {
									e++
								}), e = Math.round(e / t), (e > 1 ? C.selectedPluralText ||
									C.selectedText : C.selectedText).replace(/{count}/, e)
							})
					},
					onMarkupReady: function(e) {
						f.onMarkupReady.call(this, e), p = i(e.target), S && (i(".mbsc-fr-hdr", p)
								.attr("aria-live", "off"), x = r({}, _)), k && i(".mbsc-cal", p)
							.addClass("mbsc-cal-ev")
					},
					onDayChange: function(e) {
						var a = e.date,
							s = n(a),
							r = i(e.target),
							l = e.selected;
						if (S)
							if ("week" == C.selectType) {
								var m, d, u = s.getDay() - b;
								for (u = 0 > u ? 7 + u : u, "multiple" != C.select && (_ = {}), m =
									0; 7 > m; m++) d = c(s.getFullYear(), s.getMonth(), s
									.getDate() - u + m), l ? delete _[d] : o.objectToArray(_)
									.length / 7 < D && (_[d] = d);
								h()
							} else {
								var f = i('.mbsc-cal .mbsc-cal-day[data-full="' + r.attr(
									"data-full") + '"]', p);
								l ? (f.removeClass("mbsc-cal-day-sel").removeAttr("aria-selected")
										.find(".mbsc-cal-day-i").removeClass(M), delete _[s]) : o
									.objectToArray(_).length < D && (f.addClass("mbsc-cal-day-sel")
										.attr("aria-selected", "true").find(".mbsc-cal-day-i")
										.addClass(M), _[s] = s)
							} return C.setOnDayTap && "multiple" != C.select && "inline" != C
							.display ? (t.needsSlide = !1, t.setDate(a), t.select(), !1) : void 0
					},
					onCancel: function() {
						!t.live && S && (_ = r({}, x))
					}
				}), T
			}
		}(window, document),
		function(e, a, s) {
			var n = t,
				i = n.$,
				r = i.extend,
				o = n.util,
				l = o.datetime,
				c = l.adjustedDate,
				m = n.presets.scroller,
				d = {
					labelsShort: ["Yrs", "Mths", "Days", "Hrs", "Mins", "Secs"],
					eventText: "event",
					eventsText: "events"
				};
			n.presetShort("eventcalendar"), m.eventcalendar = function(t) {
				function a(t) {
					if (t) {
						if (V[t]) return V[t];
						var a = i('<div style="background-color:' + t + ';"></div>').appendTo("body"),
							s = e.getComputedStyle ? getComputedStyle(a[0]) : a[0].style,
							n = s.backgroundColor.replace(/rgb|rgba|\(|\)|\s/g, "").split(","),
							r = .299 * n[0] + .587 * n[1] + .114 * n[2],
							o = r > 130 ? "#000" : "#fff";
						return a.remove(), V[t] = o, o
					}
				}

				function o(e) {
					var t = F.labelsShort,
						a = Math.abs(e) / 1e3,
						s = a / 60,
						n = s / 60,
						i = n / 24,
						r = i / 365;
					return 45 > a && Math.round(a) + " " + t[5].toLowerCase() || 45 > s && Math.round(s) + " " +
						t[4].toLowerCase() || 24 > n && Math.round(n) + " " + t[3].toLowerCase() || 30 > i &&
						Math.round(i) + " " + t[2].toLowerCase() || 365 > i && Math.round(i / 30) + " " + t[1]
						.toLowerCase() || Math.round(r) + " " + t[0].toLowerCase()
				}

				function u(e) {
					return e.sort(function(e, t) {
						var a = e.d || e.start,
							s = t.d || t.start,
							n = a.getTime ? e.start && e.end && e.start.toDateString() !== e.end
							.toDateString() ? 1 : a.getTime() : 0,
							i = s.getTime ? t.start && t.end && t.start.toDateString() !== t.end
							.toDateString() ? 1 : s.getTime() : 0;
						return n - i
					})
				}

				function h(e) {
					var t, a = i(".mbsc-cal-c", x)[0].offsetHeight,
						s = e[0].offsetHeight,
						n = e[0].offsetWidth,
						r = e.offset().top - i(".mbsc-cal-c", x).offset().top,
						o = e.closest(".mbsc-cal-row").index() < 2;
					t = T.addClass("mbsc-cal-events-t").css({
							top: o ? r + s : "0",
							bottom: o ? "0" : a - r
						}).addClass("mbsc-cal-events-v").height(), T.css(o ? "bottom" : "top", "auto")
						.removeClass("mbsc-cal-events-t"), M.css("max-height", t), C.refresh(), C.scroll(0), o ?
						T.addClass("mbsc-cal-events-b") : T.removeClass("mbsc-cal-events-b"), i(
							".mbsc-cal-events-arr", T).css("left", e.offset().left - T.offset().left + n / 2)
				}

				function f(e, s) {
					var n = w[e];
					if (n) {
						var r, c, m, d, f, p, b = '<ul class="mbsc-cal-event-list">';
						k = 0, y = s, s.addClass(L).find(".mbsc-cal-day-i").addClass(H), s.hasClass(I) && s
							.attr("data-hl", "true").removeClass(I), u(n), i.each(n, function(e, t) {
								d = t.d || t.start, f = t.start && t.end && t.start.toDateString() !== t.end
									.toDateString(), m = t.color, p = a(m), r = "", c = "", d.getTime && (
										r = l.formatDate((f ? "MM d yy " : "") + F.timeFormat, d)), t.end &&
									(c = l.formatDate((f ? "MM d yy " : "") + F.timeFormat, t.end)), b +=
									'<li role="button" aria-label="' + t.text + (r ? ", " + F.fromText +
										" " + r : "") + (c ? ", " + F.toText + " " + c : "") +
									'" class="mbsc-cal-event"><div class="mbsc-cal-event-color" style="' + (
										m ? "background:" + m + ";" : "") +
									'"></div><div class="mbsc-cal-event-text">' + (d.getTime && !f ?
										'<div class="mbsc-cal-event-time">' + l.formatDate(F.timeFormat,
										d) + "</div>" : "") + t.text + "</div>" + (t.start && t.end ?
										'<div class="mbsc-cal-event-dur">' + o(t.end - t.start) + "</div>" :
										"") + "</li>"
							}), b += "</ul>", S.html(b), t.trigger("onEventBubbleShow", {
								target: y[0],
								eventList: T[0]
							}), h(y), t.tap(i(".mbsc-cal-event", S), function(a) {
								C.scrolled || t.trigger("onEventSelect", {
									domEvent: a,
									event: n[i(this).index()],
									date: e
								})
							}), D = !0
					}
				}

				function p() {
					T && T.removeClass("mbsc-cal-events-v"), y && (y.removeClass(L).find(".mbsc-cal-day-i")
						.removeClass(H), y.attr("data-hl") && y.removeAttr("data-hl").addClass(I)), D = !1
				}

				function b() {
					p(), t.redraw()
				}

				function v(e) {
					return c(e.getFullYear(), e.getMonth(), e.getDate())
				}
				var g, x, T, y, w, C, M, S, D, k, _, A, V = {},
					N = r({}, t.settings),
					F = r(t.settings, d, N),
					L = "mbsc-cal-day-sel mbsc-cal-day-ev",
					I = "mbsc-cal-day-hl",
					H = F.activeClass || "",
					O = F.showEventCount,
					P = 0,
					E = r(!0, [], F.data);
				return _ = m.calbase.call(this, t), g = r({}, _), i.each(E, function(e, t) {
					t._id === s && (t._id = P++)
				}), t.onGenMonth = function(e, a) {
					w = t.prepareObj(E, e, a)
				}, t.getDayProps = function(e) {
					var t, s = w[e] ? w[e] : !1,
						n = s ? w[e].length + " " + (w[e].length > 1 ? F.eventsText : F.eventText) : 0,
						r = s && s[0] && s[0].color,
						o = O && n ? a(r) : "",
						l = "",
						c = "";
					if (s) {
						for (t = 0; t < s.length; t++) s[t].icon && (l += '<span class="mbsc-ic mbsc-ic-' +
							s[t].icon + '"' + (s[t].text ? "" : s[t].color ? ' style="color:' + s[t]
								.color + ';"' : "") + "></span>\n");
						for (c = '<div class="mbsc-cal-day-m"><div class="mbsc-cal-day-m-t">', t = 0; t < s
							.length; t++) c += '<div class="mbsc-cal-day-m-c"' + (s[t].color ?
							' style="background:' + s[t].color + ';"' : "") + "></div>";
						c += "</div></div>"
					}
					return {
						marked: s,
						selected: !1,
						cssClass: s ? "mbsc-cal-day-marked" : "",
						ariaLabel: O ? n : "",
						markup: O && n ?
							'<div class="mbsc-cal-day-txt-c"><div class="mbsc-cal-day-txt" title="' + i(
								"<div>" + n + "</div>").text() + '"' + (r ? ' style="background:' + r +
								";color:" + o + ';text-shadow:none;"' : "") + ">" + l + n + "</div></div>" :
							O && l ? '<div class="mbsc-cal-day-ic-c">' + l + "</div>" : s ? c : ""
					}
				}, t.addEvent = function(e) {
					var t = [];
					return e = r(!0, [], i.isArray(e) ? e : [e]), i.each(e, function(e, a) {
						a._id === s && (a._id = P++), E.push(a), t.push(a._id)
					}), b(), t
				}, t.removeEvent = function(e) {
					e = i.isArray(e) ? e : [e], i.each(e, function(e, t) {
						i.each(E, function(e, a) {
							return a._id === t ? (E.splice(e, 1), !1) : void 0
						})
					}), b()
				}, t.getEvents = function(e) {
					var a;
					return e ? (e.setHours(0, 0, 0, 0), a = t.prepareObj(E, e.getFullYear(), e.getMonth()),
						a[e] ? u(a[e]) : []) : r(!0, [], E)
				}, t.setEvents = function(e) {
					var t = [];
					return E = r(!0, [], e), i.each(E, function(e, a) {
						a._id === s && (a._id = P++), t.push(a._id)
					}), b(), t
				}, r(_, {
					highlight: !1,
					outerMonthChange: !1,
					headerText: !1,
					buttons: "inline" !== F.display ? ["cancel"] : F.buttons,
					onMarkupReady: function(e) {
						g.onMarkupReady.call(this, e), x = i(e.target), O && i(".mbsc-cal", x)
							.addClass("mbsc-cal-ev"), x.addClass("mbsc-cal-em"), T = i(
								'<div class="mbsc-cal-events ' + (F.eventBubbleClass || "") +
								'"><div class="mbsc-cal-events-arr"></div><div class="mbsc-cal-events-i"><div class="mbsc-cal-events-sc"></div></div></div>'
								).appendTo(i(".mbsc-cal-c", x)), M = i(".mbsc-cal-events-i", T), S =
							i(".mbsc-cal-events-sc", T), C = new n.classes.ScrollView(M[0]), D = !1,
							t.tap(M, function() {
								C.scrolled || p()
							})
					},
					onMonthChange: function() {
						p()
					},
					onSelectShow: function() {
						p()
					},
					onMonthLoaded: function() {
						A && (f(A.d, i('.mbsc-cal-day-v[data-full="' + A.full +
							'"]:not(.mbsc-cal-day-diff)', x)), A = !1)
					},
					onDayChange: function(e) {
						var a = e.date,
							s = v(a),
							n = i(e.target);
						return p(), n.hasClass("mbsc-cal-day-ev") || setTimeout(function() {
							t.changing ? A = {
								d: s,
								full: n.attr("data-full")
							} : f(s, n)
						}, 10), !1
					},
					onCalResize: function() {
						D && h(y)
					}
				}), _
			}
		}(window, document),
		function() {
			function e(e) {
				var t = [Math.round(e.r).toString(16), Math.round(e.g).toString(16), Math.round(e.b).toString(16)];
				return l.each(t, function(e, a) {
					1 == a.length && (t[e] = "0" + a)
				}), "#" + t.join("")
			}

			function a(e) {
				return e = parseInt(e.indexOf("#") > -1 ? e.substring(1) : e, 16), {
					r: e >> 16,
					g: (65280 & e) >> 8,
					b: 255 & e
				}
			}

			function s(e) {
				var t, a, s, n = e.h,
					i = 255 * e.s / 100,
					r = 255 * e.v / 100;
				if (0 === i) t = a = s = r;
				else {
					var o = r,
						l = (255 - i) * r / 255,
						c = (o - l) * (n % 60) / 60;
					360 == n && (n = 0), 60 > n ? (t = o, s = l, a = l + c) : 120 > n ? (a = o, s = l, t = o - c) :
						180 > n ? (a = o, t = l, s = l + c) : 240 > n ? (s = o, t = l, a = o - c) : 300 > n ? (s =
							o, a = l, t = l + c) : 360 > n ? (t = o, a = l, s = o - c) : t = a = s = 0
				}
				return {
					r: t,
					g: a,
					b: s
				}
			}

			function n(e) {
				var t, a, s = 0,
					n = Math.min(e.r, e.g, e.b),
					i = Math.max(e.r, e.g, e.b),
					r = i - n;
				return a = i, t = i ? 255 * r / i : 0, s = t ? e.r == i ? (e.g - e.b) / r : e.g == i ? 2 + (e.b - e
						.r) / r : 4 + (e.r - e.g) / r : -1, s *= 60, 0 > s && (s += 360), t *= 100 / 255, a *= 100 /
					255, {
						h: s,
						s: t,
						v: a
					}
			}

			function i(t) {
				return e(s(t))
			}

			function r(e) {
				return n(a(e))
			}
			var o = t,
				l = o.$,
				c = o.util.prefix,
				m = o.presets.scroller,
				d = {
					preview: !0,
					previewText: !0,
					label: "Color",
					refineLabel: "Refine",
					step: 10,
					nr: 10,
					format: "hex",
					hueText: "Hue",
					saturationText: "Saturation",
					valueText: "Value"
				};
			o.presetShort("color"), m.color = function(e) {
				function t(e, t) {
					return Math.floor(e / t) * t
				}

				function a(e) {
					return isNaN(+e) ? 0 : +e
				}

				function o(e) {
					if ("hsv" == x) return e.join(",");
					if ("rgb" == x) {
						var t = s({
							h: e[0],
							s: e[1],
							v: e[2]
						});
						return Math.round(t.r) + "," + Math.round(t.g) + "," + Math.round(t.b)
					}
					return i({
						h: e[0],
						s: e[1],
						v: e[2]
					})
				}

				function m(e, t, a) {
					e[0].style.backgroundImage = c + ("-webkit-" == c ?
						"gradient(linear,left top,left bottom,from(" + t + "),to(" + a + "))" :
						"linear-gradient(" + t + "," + a + ")")
				}

				function u(t, a) {
					var n = e._tempWheelArray;
					if (1 !== a && 2 !== a && m(l(".mbsc-sc-whl-sc", t).eq(1), i({
							h: n[0],
							s: 0,
							v: 100
						}), i({
							h: n[0],
							s: 100,
							v: 100
						})), 2 !== a && m(l(".mbsc-sc-whl-sc", t).eq(2), i({
							h: n[0],
							s: n[1],
							v: 0
						}), i({
							h: n[0],
							s: n[1],
							v: 100
						})), T) {
						var r = s({
								h: n[0],
								s: n[1],
								v: n[2]
							}),
							c = .299 * r.r + .587 * r.g + .114 * r.b;
						l(".mbsc-color-preview", t).attr("style", "background:" + i({
							h: n[0],
							s: n[1],
							v: n[2]
						}) + ";color:" + (c > 130 ? "#000" : "#fff")).text(y ? o(n) : "")
					}
				}

				function h() {
					for (var e = 0, t = {
							data: [],
							label: w
						}, a = {
							circular: !1,
							data: [],
							label: C
						}, s = {
							circular: !1,
							data: [],
							label: M
						}; 360 > e; e += 3) t.data.push({
						value: e,
						label: e,
						display: '<div class="mbsc-color-itm" style="background:' + i({
							h: e,
							s: 100,
							v: 100
						}) + '"><div class="mbsc-color-itm-a"></div></div>'
					});
					for (e = 0; 101 > e; e += 1) a.data.push({
						value: e,
						label: e,
						display: '<div class="mbsc-color-itm"><div class="mbsc-color-itm-a"></div></div>'
					}), s.data.push({
						value: e,
						label: e,
						display: '<div class="mbsc-color-itm"><div class="mbsc-color-itm-a"></div></div>'
					});
					return [
						[t, a, s]
					]
				}
				var f, p = l.extend({}, e.settings),
					b = l.extend(e.settings, d, p),
					v = l.isArray(b.colors) ? b.colors : [b.colors],
					g = b.defaultValue || v[0],
					x = b.format,
					T = b.preview,
					y = b.previewText,
					w = b.hueText,
					C = b.saturationText,
					M = b.valueText;
				return f = h(), {
					minWidth: 70,
					height: 15,
					rows: 13,
					speedUnit: .006,
					timeUnit: .05,
					showLabel: !0,
					scroll3d: !1,
					wheels: f,
					compClass: "mbsc-color",
					parseValue: function(e) {
						var s, i;
						return e = e || g, e ? ("hsv" == x ? (e = e.split(","), i = {
							h: a(e[0]),
							s: a(e[1]),
							v: a(e[2])
						}) : "rgb" == x ? (s = e.split(","), i = n({
							r: a(s[0]),
							g: a(s[1]),
							b: a(s[2])
						})) : (e = e.replace("#", ""), 3 == e.length && (e = e[0] + e[0] + e[
							1] + e[1] + e[2] + e[2]), i = r(e)), [t(Math.round(i.h), 3), Math
							.round(i.s), Math.round(i.v)
						]) : [0, 100, 100]
					},
					formatValue: o,
					onBeforeShow: function() {
						T && (b.headerText = !1)
					},
					onMarkupReady: function(e) {
						var t = l(e.target);
						T && t.find(".mbsc-sc-whl-gr-c").before(
							'<div class="mbsc-color-preview"></div>'), u(t)
					},
					validate: function(t) {
						e._isVisible && u(e._markup, t.index)
					}
				}
			}, o.util.color = {
				hsv2hex: i,
				hsv2rgb: s,
				rgb2hsv: n,
				rgb2hex: e,
				hex2rgb: a,
				hex2hsv: r
			}
		}(),
		function(e, a, s) {
			var n, i = t,
				r = i.$,
				o = r.extend,
				l = i.classes,
				c = i.util,
				m = c.prefix,
				d = c.jsPrefix,
				u = c.getCoord,
				h = c.testTouch,
				f = c.vibrate,
				p = 1,
				b = function() {},
				v = e.requestAnimationFrame || function(e) {
					e()
				},
				g = e.cancelAnimationFrame || b,
				x = "webkitAnimationEnd animationend",
				T = "transparent";
			l.ListView = function(i, y) {
				function w() {
					wa = !1, fa = !1, dt = 0, Na = 0, Fa = new Date, ea = vt.width(), Tt = K(vt), sa = Tt.index(
						ta), aa = ta[0].offsetHeight, ts = ta[0].offsetTop, Wa = $a[ta.attr("data-type") ||
						"defaults"], Va = Wa.stages
				}

				function C(e) {
					var s;
					"touchstart" === e.type && (pa = !0, clearTimeout(ba)), !h(e, this) || lt || Qa || n ||
						is || !t.vKMaI || (lt = !0, ut = !0, La = u(e, "X"), Ia = u(e, "Y"), Dt = 0, kt = 0,
							ta = r(this), s = ta, w(), Ya = Sa.onItemTap || Wa.tap || ta.hasClass(
								"mbsc-lv-parent") || ta.hasClass("mbsc-lv-back"), ia = Za.offset().top, na = ta
							.offset().top, Ya && (mt = setTimeout(function() {
								s.addClass("mbsc-lv-item-active"), Ht("onItemActivate", {
									target: s[0],
									domEvent: e
								})
							}, 120)), Ga.sortable && !ta.hasClass("mbsc-lv-back") && (Ga.sortable.group || (ua =
									ta.nextUntil(".mbsc-lv-gr-title").filter(".mbsc-lv-item"), va = ta
									.prevUntil(".mbsc-lv-gr-title").filter(".mbsc-lv-item")), oa = (Ga.sortable
									.group ? vt.children("li").eq(0) : va.length ? va.eq(-1) : ta)[0]
								.offsetTop - ts, ra = (Ga.sortable.group ? vt.children("li").eq(-1) : ua
									.length ? ua.eq(-1) : ta)[0].offsetTop - ts, Ga.sortable.handle ? r(e
									.target).hasClass("mbsc-lv-handle") && (clearTimeout(mt), "Moz" === d ? (e
									.preventDefault(), A()) : ja = setTimeout(function() {
									A()
								}, 100)) : ja = setTimeout(function() {
									Ot.appendTo(ta), Ot[0].style[d + "Animation"] = "mbsc-lv-fill " + (Sa
										.sortDelay - 100) + "ms linear", clearTimeout(Ft), clearTimeout(
										mt), ut = !1, ja = setTimeout(function() {
										Ot[0].style[d + "Animation"] = "", A()
									}, Sa.sortDelay - 80)
								}, 80)), "mousedown" == e.type && r(a).on("mousemove", M).on("mouseup", S))
				}

				function M(e) {
					var t = !1,
						a = !0;
					if (lt)
						if (Lt = u(e, "X"), It = u(e, "Y"), Dt = Lt - La, kt = It - Ia, clearTimeout(Ft), Vt ||
							Oa || Da || ta.hasClass("mbsc-lv-back") || (Math.abs(kt) > 10 ? (Da = !0, e.type =
									"mousemove" == e.type ? "mouseup" : "touchend", S(e), clearTimeout(mt)) :
								Math.abs(Dt) > 7 ? D() : "touchmove" === e.type && (Ft = setTimeout(function() {
									e.type = "touchend", S(e)
								}, 300))), Oa) e.preventDefault(), dt = Dt / ea * 100, k();
						else if (Vt) {
						e.preventDefault();
						var s, n = Ba.scrollTop(),
							i = Math.max(oa, Math.min(kt + Ua, ra)),
							r = jt ? na - Ka + n - Ua : na;
						r + i + aa > qa + n ? (Ba.scrollTop(r + i - qa + aa), s = !0) : n > r + i && (Ba
							.scrollTop(r + i), s = !0), s && (Ua += Ba.scrollTop() - n), ma && (Ga.sortable
							.multiLevel && ca.hasClass("mbsc-lv-parent") ? ts + aa / 4 + i > ma ? t = !0 :
							ts + aa - aa / 4 + i > ma && (_t = ca.addClass("mbsc-lv-item-hl"), a = !1) :
							ts + aa / 2 + i > ma && (ca.hasClass("mbsc-lv-back") ? Ga.sortable.multiLevel &&
								(At = ca.addClass("mbsc-lv-item-hl"), a = !1) : t = !0), t && (ga
								.insertAfter(ca), xa = ca, ca = Z(ca, "next"), Ta = ma, ma = ca.length &&
								ca[0].offsetTop, bt++)), !t && Ta && (Ga.sortable.multiLevel && xa.hasClass(
								"mbsc-lv-parent") ? Ta > ts + aa - aa / 4 + i ? t = !0 : Ta > ts + aa / 4 +
							i && (_t = xa.addClass("mbsc-lv-item-hl"), a = !1) : Ta > ts + aa / 2 + i && (xa
								.hasClass("mbsc-lv-back") ? Ga.sortable.multiLevel && (At = xa.addClass(
									"mbsc-lv-item-hl"), a = !1) : t = !0), t && (ga.insertBefore(xa), ca =
								xa, xa = Z(xa, "prev"), ma = Ta, Ta = xa.length && xa[0].offsetTop + xa[0]
								.offsetHeight, bt--)), a && (_t && (_t.removeClass("mbsc-lv-item-hl"),
							_t = !1), At && (At.removeClass("mbsc-lv-item-hl"), At = !1)), t && Ht(
							"onSortChange", [ta, bt]), z(ta, i), Ht("onSort", [ta, bt])
					} else(Math.abs(Dt) > 5 || Math.abs(kt) > 5) && j()
				}

				function S(e) {
					var t, s, n, i, o = ta;
					lt && (lt = !1, j(), "mouseup" == e.type && r(a).off("mousemove", M).off("mouseup", S),
						Da || (ba = setTimeout(function() {
							pa = !1
						}, 300)), (Oa || Da || Vt) && (fa = !0), Oa ? _() : Vt ? (n = vt, _t ? (R(ta
							.detach()), s = ns[_t.attr("data-ref")], bt = K(s.child).length, _t
							.removeClass("mbsc-lv-item-hl"), Sa.navigateOnDrop ? nt(_t, function() {
								Ga.add(null, ta, null, null, _t, !0), at(ta), V(ta, sa, n, !0)
							}) : (Ga.add(null, ta, null, null, _t, !0), V(ta, sa, n, !0))) : At ? (R(ta
								.detach()), s = ns[At.attr("data-back")], bt = K(s.parent).index(s
							.item) + 1, At.removeClass("mbsc-lv-item-hl"), Sa.navigateOnDrop ? nt(At,
								function() {
									Ga.add(null, ta, bt, null, vt, !0), at(ta), V(ta, sa, n, !0)
								}) : (Ga.add(null, ta, bt, null, s.parent, !0), V(ta, sa, n, !0))) : (
							t = ga[0].offsetTop - ts, z(ta, t, 6 * Math.abs(t - Math.max(oa, Math.min(
								kt + Ua, ra))), function() {
								R(ta), ta.insertBefore(ga), V(ta, sa, n, bt !== sa)
							})), Vt = !1) : !Da && Math.abs(Dt) < 5 && Math.abs(kt) < 5 && (Wa.tap && (i =
							Wa.tap.call(Xa, {
								target: ta,
								index: sa,
								domEvent: e
							}, Ga)), Ya && ("touchend" === e.type && c.preventClick(), ta.addClass(
							"mbsc-lv-item-active"), Ht("onItemActivate", {
							target: ta[0],
							domEvent: e
						})), i = Ht("onItemTap", {
							target: ta[0],
							index: sa,
							domEvent: e
						}), i !== !1 && nt(ta)), clearTimeout(mt), setTimeout(function() {
							o.removeClass("mbsc-lv-item-active"), Ht("onItemDeactivate", {
								target: o[0]
							})
						}, 100), Da = !1, yt = null)
				}

				function D() {
					Oa = B(Wa.swipe, {
						target: ta[0],
						index: sa,
						direction: Dt > 0 ? "right" : "left"
					}), Oa && (j(), clearTimeout(mt), Wa.actions ? (ct = tt(Wa, Dt), la.html(Wa.icons)
						.show().children().css("width", ct + "%"), Zt.hide(), r(".mbsc-lv-ic-m", Qt)
						.removeClass("mbsc-lv-ic-disabled"), r(Wa.leftMenu).each(I), r(Wa.rightMenu)
						.each(I)) : (Zt.show(), la.hide(), wt = Wa.start + (Dt > 0 ? 0 : 1), ya = Va[
						wt - 1], da = Va[wt]), ta.addClass("mbsc-lv-item-swiping").removeClass(
						"mbsc-lv-item-active"), za.css("line-height", aa + "px"), Qt.css({
						top: ts,
						height: aa,
						backgroundColor: Q(Dt)
					}).addClass("mbsc-lv-stage-c-v").appendTo(vt.parent()), Sa.iconSlide && ta.append(
						Zt), Ht("onSlideStart", {
						target: ta[0],
						index: sa
					}))
				}

				function k() {
					var e = !1;
					Ma || (Wa.actions ? Qt.attr("class", "mbsc-lv-stage-c-v mbsc-lv-stage-c mbsc-lv-" + (0 >
						dt ? "right" : "left")) : (ya && dt <= ya.percent ? (wt--, da = ya, ya = Va[wt],
							e = !0) : da && dt >= da.percent && (wt++, ya = da, da = Va[wt], e = !0),
						e && (yt = dt > 0 ? ya : da, yt && (W(yt, Sa.iconSlide), Ht("onStageChange", {
							target: ta[0],
							index: sa,
							stage: yt
						})))), ka || (Ma = !0, Ca = v(P)))
				}

				function _(e) {
					var t, s, i, o = !1,
						l = !0;
					g(Ca), Ma = !1, ka || P(), Wa.actions ? Math.abs(dt) > 10 && ct && (Y(ta, 0 > dt ? -ct : ct,
						200), o = !0, n = !0, ht = ta, ft = sa, r(a).on(
						"touchstart.mbsc-lv-conf mousedown.mbsc-lv-conf",
						function(t) {
							t.preventDefault(), E(ta, !0, e)
						})) : dt && (Sa.quickSwipe && !ka && (i = new Date - Fa, t = 300 > i && -50 > Dt,
						s = 300 > i && Dt > 50, t ? (wa = !0, yt = Wa.left, W(yt, Sa.iconSlide)) : s &&
						(wa = !0, yt = Wa.right, W(yt, Sa.iconSlide))), yt && yt.action && (St = B(yt
						.disabled, {
							target: ta[0],
							index: sa
						}), St || (o = !0, n = ka || B(yt.confirm, {
						target: ta[0],
						index: sa
					}), n ? (Y(ta, (0 > dt ? -1 : 1) * Zt[0].offsetWidth * 100 / ea, 200, !0),
						O(yt, ta, sa, !1, e)) : H(yt, ta, sa, e)))), o || E(ta, l, e), Oa = !1
				}

				function A() {
					Vt = !0, _t = !1, At = !1, Ua = 0, bt = sa, Sa.vibrate && f(), ca = Z(ta, "next"), ma = ca
						.length && ca[0].offsetTop, xa = Z(ta, "prev"), Ta = xa.length && xa[0].offsetTop + xa[
							0].offsetHeight, ga.height(aa).insertAfter(ta), ta.css({
							top: ts
						}).addClass("mbsc-lv-item-dragging").removeClass("mbsc-lv-item-active").appendTo(Nt),
						Ht("onSortStart", {
							target: ta[0],
							index: bt
						})
				}

				function V(e, t, a, s) {
					e.removeClass("mbsc-lv-item-dragging"), ga.remove(), Ht("onSortEnd", {
						target: e[0],
						index: bt
					}), Sa.vibrate && f(), s && (Ga.addUndoAction(function(s) {
						Ga.move(e, t, null, s, a, !0)
					}, !0), Ht("onSortUpdate", {
						target: e[0],
						index: bt
					}))
				}

				function N() {
					pa || (clearTimeout(Ut), n && r(a).trigger("touchstart"), Rt && (Ga.close(Jt, Bt), Rt = !1,
						Jt = null))
				}

				function F() {
					clearTimeout(Ct), Ct = setTimeout(function() {
						qa = Ba[0].innerHeight || Ba.innerHeight(), Ka = jt ? Ba.offset().top : 0, lt &&
							(ts = ta[0].offsetTop, aa = ta[0].offsetHeight, Qt.css({
								top: ts,
								height: aa
							}))
					}, 200)
				}

				function L() {
					if (Vt || !lt) {
						var e, t = Ba.scrollTop(),
							a = Za.offset().top,
							s = Za[0].offsetHeight,
							n = jt ? Ba.offset().top : t;
						r(".mbsc-lv-gr-title", Za).each(function(t, a) {
							r(a).offset().top < n && (e = a)
						}), n > a && a + s > n ? Et.show().empty().append(r(e).clone()) : Et.hide()
					}
				}

				function I(e, t) {
					B(t.disabled, {
						target: ta[0],
						index: sa
					}) && r(".mbsc-ic-" + t.icon, Qt).addClass("mbsc-lv-ic-disabled")
				}

				function H(e, t, a, s) {
					var n, i = {
						icon: "undo2",
						text: Sa.undoText,
						color: "#b1b1b1",
						action: function() {
							Ga.undo()
						}
					};
					e.undo && (Ga.startActionTrack(), r.isFunction(e.undo) && Ga.addUndoAction(function() {
						e.undo.call(Xa, t, Ga, a)
					}), Ja = t.attr("data-ref")), n = e.action.call(Xa, {
						target: t[0],
						index: a
					}, Ga), e.undo ? (Ga.endActionTrack(), n !== !1 && Y(t, +t.attr("data-pos") < 0 ? -100 :
						100, 200), ga.height(aa).insertAfter(t), t.css("top", ts).addClass(
						"mbsc-lv-item-undo"), la.hide(), Zt.show(), Qt.append(Zt), W(i), O(i, t, a, !0,
						s)) : E(t, n, s)
				}

				function O(e, t, s, i, o) {
					var l, m;
					n = !0, r(a).off(".mbsc-lv-conf").on("touchstart.mbsc-lv-conf mousedown.mbsc-lv-conf",
						function(e) {
							e.preventDefault(), i && J(t), E(t, !0, o)
						}), Mt || Zt.off(".mbsc-lv-conf").on(
						"touchstart.mbsc-lv-conf mousedown.mbsc-lv-conf",
						function(e) {
							e.stopPropagation(), l = u(e, "X"), m = u(e, "Y")
						}).on("touchend.mbsc-lv-conf mouseup.mbsc-lv-conf", function(a) {
						a.preventDefault(), "touchend" === a.type && c.preventClick(), Math.abs(u(a,
							"X") - l) < 10 && Math.abs(u(a, "Y") - m) < 10 && (H(e, t, s, o), i && (
							Ra = null, J(t)))
					})
				}

				function P() {
					Y(ta, Na + 100 * Dt / ea), Ma = !1
				}

				function E(e, t, s) {
					r(a).off(".mbsc-lv-conf"), Zt.off(".mbsc-lv-conf"), t !== !1 ? Y(e, 0, "0" !== e.attr(
						"data-pos") ? 200 : 0, !1, function() {
						$(e, s), R(e)
					}) : $(e, s), n = !1
				}

				function Y(e, t, a, s, n) {
					t = Math.max("right" == Oa ? 0 : -100, Math.min(t, "left" == Oa ? 0 : 100)), Ha = e[0]
						.style, e.attr("data-pos", t), Ha[d + "Transform"] = "translate3d(" + (s ? ea * t /
							100 + "px" : t + "%") + ",0,0)", Ha[d + "Transition"] = m + "transform " + (a ||
						0) + "ms", n && (Qa++, setTimeout(function() {
							n(), Qa--
						}, a)), dt = t
				}

				function z(e, t, a, s) {
					t = Math.max(oa, Math.min(t, ra)), Ha = e[0].style, Ha[d + "Transform"] = "translate3d(0," +
						t + "px,0)", Ha[d + "Transition"] = m + "transform " + (a || 0) + "ms ease-out", s && (
							Qa++, setTimeout(function() {
								s(), Qa--
							}, a))
				}

				function j() {
					clearTimeout(ja), !ut && Ga.sortable && (ut = !0, Ot.remove())
				}

				function W(e, t) {
					var a = B(e.text, {
						target: ta[0],
						index: sa
					}) || "";
					B(e.disabled, {
							target: ta[0],
							index: sa
						}) ? Qt.addClass("mbsc-lv-ic-disabled") : Qt.removeClass("mbsc-lv-ic-disabled"), Qt.css(
							"background-color", e.color || (0 === e.percent ? Q(dt) : T)), Zt.attr("class",
							"mbsc-lv-ic-c mbsc-lv-ic-" + (t ? "move-" : "") + (0 > dt ? "right" : "left")), Xt
						.attr("class", " mbsc-lv-ic-s mbsc-lv-ic mbsc-ic mbsc-ic-" + (e.icon || "none")), za
						.attr("class", "mbsc-lv-ic-text" + (e.icon ? "" : " mbsc-lv-ic-text-only") + (a ? "" :
							" mbsc-lv-ic-only")).html(a || "&nbsp;"), Sa.animateIcons && (wa ? Xt.addClass(
							"mbsc-lv-ic-v") : setTimeout(function() {
							Xt.addClass("mbsc-lv-ic-a")
						}, 10))
				}

				function $(e, t) {
					lt || (Xt.attr("class", "mbsc-lv-ic-s mbsc-lv-ic mbsc-ic mbsc-ic-none"), Qt.attr("style",
						"").removeClass("mbsc-lv-stage-c-v"), za.html("")), Qt.removeClass(
						"mbsc-lv-left mbsc-lv-right"), e && (Ht("onSlideEnd", {
						target: e[0],
						index: sa
					}), t && t())
				}

				function J(e) {
					e.css("top", "").removeClass("mbsc-lv-item-undo"), Ra ? Ga.animate(ga, "collapse",
					function() {
						ga.remove()
					}) : ga.remove(), $(), Ja = null, Ra = null
				}

				function R(e) {
					Ha = e[0].style, Ha[d + "Transform"] = "", Ha[d + "Transition"] = "", Ha.top = "", e
						.removeClass("mbsc-lv-item-swiping")
				}

				function B(e, t) {
					return r.isFunction(e) ? e.call(this, t, Ga) : e
				}

				function q(e) {
					var t;
					if (e.attr("data-ref") || (t = p++, e.attr("data-ref", t), ns[t] = {
							item: e,
							child: e.children("ul,ol"),
							parent: e.parent(),
							ref: e.parent()[0] === Xa ? null : e.parent().parent().attr("data-ref")
						}), e.addClass("mbsc-lv-item"), Ga.sortable.handle && "list-divider" != e.attr(
							"data-role") && !e.children(".mbsc-lv-handle-c").length && e.append(Wt), Sa
						.enhance && !e.hasClass("mbsc-lv-item-enhanced")) {
						var a = e.attr("data-icon"),
							s = e.find("img").eq(0).addClass("mbsc-lv-img");
						s.is(":first-child") ? e.addClass("mbsc-lv-img-" + (Sa.rtl ? "right" : "left")) : s
							.length && e.addClass("mbsc-lv-img-" + (Sa.rtl ? "left" : "right")), e.addClass(
								"mbsc-lv-item-enhanced").children().each(function(e, t) {
								t = r(t), t.is("p, h1, h2, h3, h4, h5, h6") && t.addClass("mbsc-lv-txt")
							}), a && e.addClass("mbsc-lv-item-ic-" + (e.attr("data-icon-align") || (Sa.rtl ?
								"right" : "left"))).append('<div class="mbsc-lv-item-ic mbsc-ic mbsc-ic-' + a +
								'"></div')
					}
				}

				function U(e) {
					r("li", e).not(".mbsc-lv-item").each(function() {
							q(r(this))
						}), r('li[data-role="list-divider"]', e).removeClass("mbsc-lv-item").addClass(
							"mbsc-lv-gr-title"), r("ul,ol", e).not(".mbsc-lv").addClass("mbsc-lv").prepend(Kt)
						.parent().addClass("mbsc-lv-parent").prepend(Gt), r(".mbsc-lv-back", e).each(
					function() {
							r(this).attr("data-back", r(this).parent().parent().attr("data-ref"))
						})
				}

				function K(e) {
					return e.children("li").not(".mbsc-lv-back").not(".mbsc-lv-removed").not(".mbsc-lv-ph")
				}

				function G(e) {
					return "object" != typeof e && (e = r('li[data-id="' + e + '"]', pt)), r(e)
				}

				function X(e) {
					for (var t = 0, a = ns[e.attr("data-ref")]; a.ref;) t++, a = ns[a.ref];
					return t
				}

				function Z(e, t) {
					for (e = e[t](); e.length && (!e.hasClass("mbsc-lv-item") || e.hasClass("mbsc-lv-ph") || e
							.hasClass("mbsc-lv-item-dragging"));) {
						if (!Ga.sortable.group && e.hasClass("mbsc-lv-gr-title")) return !1;
						e = e[t]()
					}
					return e
				}

				function Q(e) {
					return (e > 0 ? Wa.right : Wa.left).color || T
				}

				function et(e) {
					return c.isNumeric(e) ? e + "" : 0
				}

				function tt(e, t) {
					return +(0 > t ? et((e.actionsWidth || 0).right) || et(e.actionsWidth) || et(Sa.actionsWidth
						.right) || et(Sa.actionsWidth) : et((e.actionsWidth || 0).left) || et(e
						.actionsWidth) || et(Sa.actionsWidth.left) || et(Sa.actionsWidth))
				}

				function at(e, t) {
					if (e) {
						var a = Ba.scrollTop(),
							s = e.is(".mbsc-lv-item") ? e[0].offsetHeight : 0,
							n = e.offset().top + (jt ? a - Ka : 0);
						t ? (a > n || n > a + qa) && Ba.scrollTop(n) : a > n ? Ba.scrollTop(n) : n + s > a +
							qa && Ba.scrollTop(n + s - qa / 2)
					}
				}

				function st(e, t, a, s, n) {
					var i = t.parent(),
						r = t.prev();
					s = s || b, r[0] === Zt[0] && (r = Zt.prev()), vt[0] !== t[0] ? (Ht("onNavStart", {
						level: es,
						direction: e,
						list: t[0]
					}), _a.prepend(t.addClass("mbsc-lv-v mbsc-lv-sl-new")), at(pt), it(_a,
						"mbsc-lv-sl-" + e,
						function() {
							vt.removeClass("mbsc-lv-sl-curr"), t.removeClass("mbsc-lv-sl-new").addClass(
									"mbsc-lv-sl-curr"), gt && gt.length ? vt.removeClass("mbsc-lv-v")
								.insertAfter(gt) : xt.append(vt.removeClass("mbsc-lv-v")), gt = r, xt =
								i, vt = t, at(a, n), s.call(Xa, a), Ht("onNavEnd", {
									level: es,
									direction: e,
									list: t[0]
								})
						})) : (at(a, n), s.call(Xa, a))
				}

				function nt(e, t) {
					Qa || (e.hasClass("mbsc-lv-parent") ? (es++, st("r", ns[e.attr("data-ref")].child, null,
						t)) : e.hasClass("mbsc-lv-back") && (es--, st("l", ns[e.attr("data-back")].parent,
							ns[e.attr("data-back")].item, t)))
				}

				function it(e, t, a) {
					function s() {
						clearTimeout(n), Qa--, e.off(x, s).removeClass(t), a.call(Xa, e)
					}
					var n;
					a = a || b, Sa.animation && "mbsc-lv-item-none" !== t ? (Qa++, e.on(x, s).addClass(t), n =
						setTimeout(s, 500)) : a.call(Xa, e)
				}

				function rt(e, t) {
					var a, s = e.attr("data-ref");
					a = ss[s] = ss[s] || [], t && a.push(t), e.attr("data-action") || (t = a.shift(), e.attr(
						"data-action", 1), t(function() {
						e.removeAttr("data-action"), a.length ? rt(e) : delete ss[s]
					}))
				}

				function ot(e, t, a) {
					var n, i;
					e && e.length && (n = 100 / (e.length + 2), r.each(e, function(r, l) {
						l.key === s && (l.key = Aa++), l.percent === s && (l.percent = t * n * (r +
							1), a && (i = o({}, l), i.key = Aa++, i.percent = -n * (r + 1),
							e.push(i), as[i.key] = i)), as[l.key] = l
					}))
				}
				var lt, ct, mt, dt, ut, ht, ft, pt, bt, vt, gt, xt, Tt, yt, wt, Ct, Mt, St, Dt, kt, _t, At, Vt,
					Nt, Ft, Lt, It, Ht, Ot, Pt, Et, Yt, zt, jt, Wt, $t, Jt, Rt, Bt, qt, Ut, Kt, Gt, Xt, Zt, Qt,
					ea, ta, aa, sa, na, ia, ra, oa, la, ca, ma, da, ua, ha, fa, pa, ba, va, ga, xa, Ta, ya, wa,
					Ca, Ma, Sa, Da, ka, _a, Aa, Va, Na, Fa, La, Ia, Ha, Oa, Pa, Ea, Ya, za, ja, Wa, $a, Ja, Ra,
					Ba, qa, Ua, Ka, Ga = this,
					Xa = i,
					Za = r(Xa),
					Qa = 0,
					es = 0,
					ts = 0,
					as = {},
					ss = {},
					ns = {};
				l.Base.call(this, i, y, !0), Ga.animate = function(e, t, a) {
					it(e, "mbsc-lv-item-" + t, a)
				}, Ga.add = function(e, t, a, n, i, o) {
					var l, c, m, d, u, h, f = "",
						v = i === s ? Za : G(i),
						g = v,
						x = "object" != typeof t ? r('<li data-ref="' + p++ + '" data-id="' + e + '">' + t +
							"</li>") : t,
						T = x.attr("data-pos") < 0 ? "left" : "right",
						y = x.attr("data-ref");
					n = n || b, y || (y = p++, x.addClass("mbsc-lv-item").attr("data-ref", y)), q(x), o ||
						Ga.addUndoAction(function(e) {
							d ? Ga.navigate(v, function() {
								g.remove(), v.removeClass("mbsc-lv-parent").children(
										".mbsc-lv-arr").remove(), u.child = v.children("ul,ol"),
									Ga.remove(x, null, e, !0)
							}) : Ga.remove(x, null, e, !0)
						}, !0), rt(x, function(e) {
							R(x.css("top", "").removeClass("mbsc-lv-item-undo")), v.is("li") ? (h = v
								.attr("data-ref"), v.children("ul,ol").length || (d = !0, v.append(
									"<ul></ul>"))) : h = v.children(".mbsc-lv-back").attr(
								"data-back"), u = ns[h], u && (u.child.length ? g = u.child : (v
								.addClass("mbsc-lv-parent").prepend(Gt), g = v.children("ul,ol")
								.prepend(Kt).addClass("mbsc-lv"), u.child = g, r(
									".mbsc-lv-back", v).attr("data-back", h))), ns[y] = {
								item: x,
								child: x.children("ul,ol"),
								parent: g,
								ref: h
							}, m = K(g), c = m.length, (a === s || null === a) && (a = c), o && (f =
								"mbsc-lv-item-new-" + (o ? T : "")), U(x.addClass(f)), a !== !1 && (
								c ? c > a ? x.insertBefore(m.eq(a)) : x.insertAfter(m.eq(c - 1)) : (
									l = r(".mbsc-lv-back", g), l.length ? x.insertAfter(l) : g
									.append(x))), g.hasClass("mbsc-lv-v") ? Ga.animate(x.height(x[0]
								.offsetHeight), o && Ja === y ? "none" : "expand", function(t) {
								Ga.animate(t.height(""), o ? "add-" + T : "pop-in", function(
								t) {
									n.call(Xa, t.removeClass(f)), e()
								})
							}) : (n.call(Xa, x.removeClass(f)), e()), pt.trigger("mbsc-enhance", [{
								theme: Sa.theme,
								lang: Sa.lang
							}]), Ht("onItemAdd", {
								target: x[0]
							})
						})
				}, Ga.swipe = function(e, t, a, n, i) {
					e = G(e), ta = e, Mt = n, ka = !0, lt = !0, a = a === s ? 300 : a, Dt = t > 0 ? 1 : -1,
						w(), D(), Y(e, t, a), clearTimeout(Ea), clearInterval(Pa), Pa = setInterval(
							function() {
								dt = c.getPosition(e) / ea * 100, k()
							}, 10), Ea = setTimeout(function() {
							clearInterval(Pa), dt = t, k(), _(i), Mt = !1, ka = !1, lt = !1
						}, a)
				}, Ga.openStage = function(e, t, a, s) {
					as[t] && Ga.swipe(e, as[t].percent, a, s)
				}, Ga.openActions = function(e, t, a, s) {
					e = G(e);
					var n = tt($a[e.attr("data-type") || "defaults"], "left" == t ? -1 : 1);
					Ga.swipe(e, "left" == t ? -n : n, a, s)
				}, Ga.close = function(e, t) {
					Ga.swipe(e, 0, t)
				}, Ga.remove = function(e, t, a, s) {
					var n, i;
					a = a || b, e = G(e), e.length && (i = e.parent(), n = K(i).index(e), s || (e.attr(
						"data-ref") === Ja && (Ra = !0), Ga.addUndoAction(function(t) {
						Ga.add(null, e, n, t, i, !0)
					}, !0)), rt(e, function(n) {
						t = t || e.attr("data-pos") < 0 ? "left" : "right", i.hasClass(
							"mbsc-lv-v") ? Ga.animate(e.addClass("mbsc-lv-removed"), s ?
							"pop-out" : "remove-" + t,
							function(e) {
								Ga.animate(e.height(e[0].offsetHeight), "collapse",
									function(e) {
										R(e.height("").removeClass("mbsc-lv-removed")),
											a.call(Xa, e) !== !1 && e.remove(), n()
									})
							}) : (a.call(Xa, e) !== !1 && e.remove(), n()), Ht(
							"onItemRemove", {
								target: e[0]
							})
					}))
				}, Ga.move = function(e, t, a, s, n, i) {
					e = G(e), i || Ga.startActionTrack(), Qt.append(Zt), Ga.remove(e, a, null, i), Ga.add(
						null, e, t, s, n, i), i || Ga.endActionTrack()
				}, Ga.navigate = function(e, t) {
					var a, s;
					e = G(e), a = ns[e.attr("data-ref")], s = X(e), a && (st(s >= es ? "r" : "l", a.parent,
						e, t, !0), es = s)
				}, Ga.init = function(t) {
					var a, i, o, l = Za.find("ul,ol").length ? "left" : "right",
						m = 0,
						d = "",
						h = "",
						f = "";
					Ga._init(t), o = Sa.sort || Sa.sortable, "group" === o && (o = {
							group: !1,
							multiLevel: !0
						}), o === !0 && (o = {
							group: !0,
							multiLevel: !0,
							handle: Sa.sortHandle
						}), o && o.handle === s && (o.handle = Sa.sortHandle), Ga.sortable = o || !1, a =
						"mbsc-lv-cont mbsc-lv-" + Sa.theme + (Sa.rtl ? " mbsc-lv-rtl" : "") + (Sa
							.baseTheme ? " mbsc-lv-" + Sa.baseTheme : "") + (Sa.animateIcons ?
							" mbsc-lv-ic-anim" : "") + (Sa.striped ? " mbsc-lv-alt-row " : "") + (Sa
							.fixedHeader ? " mbsc-lv-has-fixed-header " : ""), pt ? (pt.attr("class", a), Ga
							.sortable.handle && r(".mbsc-lv-handle-c", Za).remove(), r(
								"li:not(.mbsc-lv-back)", Za).removeClass("mbsc-lv-item")) : (d +=
							'<div class="mbsc-lv-multi-c"></div>', d +=
							'<div class="mbsc-lv-ic-c"><div class="mbsc-lv-ic-s mbsc-lv-ic mbsc-ic mbsc-ic-none"></div><div class="mbsc-lv-ic-text"></div></div>',
							Za.addClass("mbsc-lv mbsc-lv-v mbsc-lv-root").show(), Qt = r(
								'<div class="mbsc-lv-stage-c">' + d + "</div>"), Zt = r(".mbsc-lv-ic-c",
							Qt), la = r(".mbsc-lv-multi-c", Qt), Xt = r(".mbsc-lv-ic-s", Qt), za = r(
								".mbsc-lv-ic-text", Qt), ga = r(
							'<li class="mbsc-lv-item mbsc-lv-ph"></li>'), Ot = r(
								'<div class="mbsc-lv-fill-item"></div>'), pt = r('<div class="' + a +
								'"><ul class="mbsc-lv mbsc-lv-dummy"></ul><div class="mbsc-lv-sl-c"></div></div>'
								), jt = "body" !== Sa.context, Ba = r(jt ? Sa.context : e), Nt = r(
								".mbsc-lv-dummy", pt), pt.insertAfter(Za), Ba.on("orientationchange resize",
								F), F(), pt.on("touchstart mousedown", ".mbsc-lv-item", C).on("touchmove",
								".mbsc-lv-item", M).on("touchend touchcancel", ".mbsc-lv-item", S), Xa
							.addEventListener && Xa.addEventListener("click", function(e) {
								fa && (e.stopPropagation(), e.preventDefault(), fa = !1)
							}, !0), pt.on("touchstart mousedown", ".mbsc-lv-ic-m", function(e) {
								Mt || (e.stopPropagation(), e.preventDefault()), La = u(e, "X"), Ia = u(
									e, "Y")
							}).on("touchend mouseup", ".mbsc-lv-ic-m", function(e) {
								Mt || ("touchend" === e.type && c.preventClick(), n && !r(this)
									.hasClass("mbsc-lv-ic-disabled") && Math.abs(u(e, "X") - La) <
									10 && Math.abs(u(e, "Y") - Ia) < 10 && H((0 > dt ? Wa
										.rightMenu : Wa.leftMenu)[r(this).index()], ht, ft))
							}), _a = r(".mbsc-lv-sl-c", pt).append(Za.addClass("mbsc-lv-sl-curr")).attr(
								"data-ref", p++), vt = Za, xt = pt), Kt =
						'<li class="mbsc-lv-item mbsc-lv-back">' + Sa.backText +
						'<div class="mbsc-lv-arr mbsc-lv-ic mbsc-ic ' + Sa.leftArrowClass + '"></div></li>',
						Gt = '<div class="mbsc-lv-arr mbsc-lv-ic mbsc-ic ' + Sa.rightArrowClass +
						'"></div>', Ga.sortable.handle && (zt = Ga.sortable.handle === !0 ? l : Ga.sortable
							.handle, Wt = '<div class="mbsc-lv-handle-c mbsc-lv-item-h-' + zt +
							' mbsc-lv-handle"><div class="' + Sa.handleClass +
							' mbsc-lv-handle-bar-c mbsc-lv-handle">' + Sa.handleMarkup + "</div></div>", pt
							.addClass("mbsc-lv-handle-" + zt)), U(Za), Aa = 0, $a = Sa.itemGroups || {}, $a
						.defaults = {
							swipeleft: Sa.swipeleft,
							swiperight: Sa.swiperight,
							stages: Sa.stages,
							actions: Sa.actions,
							actionsWidth: Sa.actionsWidth
						}, r.each($a, function(e, t) {
							if (t.swipe = t.swipe !== s ? t.swipe : Sa.swipe, t.stages = t.stages || [],
								ot(t.stages, 1, !0), ot(t.stages.left, 1), ot(t.stages.right, -1), (t
									.stages.left || t.stages.right) && (t.stages = [].concat(t.stages
									.left || [], t.stages.right || [])), Pt = !1, t.stages.length || (t
									.swipeleft && t.stages.push({
										percent: -30,
										action: t.swipeleft
									}), t.swiperight && t.stages.push({
										percent: 30,
										action: t.swiperight
									})), r.each(t.stages, function(e, t) {
									return 0 === t.percent ? (Pt = !0, !1) : void 0
								}), Pt || t.stages.push({
									percent: 0
								}), t.stages.sort(function(e, t) {
									return e.percent - t.percent
								}), r.each(t.stages, function(e, a) {
									return 0 === a.percent ? (t.start = e, !1) : void 0
								}), Pt ? t.left = t.right = t.stages[t.start] : (t.left = t.stages[t
									.start - 1] || {}, t.right = t.stages[t.start + 1] || {}), t.actions
								) {
								for (t.leftMenu = t.actions.left || t.actions, t.rightMenu = t.actions
									.right || t.leftMenu, h = "", f = "", m = 0; m < t.leftMenu
									.length; m++) h += "<div " + (t.leftMenu[m].color ?
										'style="background-color: ' + t.leftMenu[m].color + '"' : "") +
									' class="mbsc-lv-ic-m mbsc-lv-ic mbsc-ic mbsc-ic-' + t.leftMenu[m]
									.icon + '">' + (t.leftMenu[m].text || "") + "</div>";
								for (m = 0; m < t.rightMenu.length; ++m) f += "<div " + (t.rightMenu[m]
										.color ? 'style="background-color: ' + t.rightMenu[m].color +
										'"' : "") + ' class="mbsc-lv-ic-m mbsc-lv-ic mbsc-ic mbsc-ic-' +
									t.rightMenu[m].icon + '">' + (t.rightMenu[m].text || "") + "</div>";
								t.actions.left && (t.swipe = t.actions.right ? t.swipe : "right"), t
									.actions.right && (t.swipe = t.actions.left ? t.swipe : "left"), t
									.icons = '<div class="mbsc-lv-multi mbsc-lv-multi-ic-left">' + h +
									'</div><div class="mbsc-lv-multi mbsc-lv-multi-ic-right">' + f +
									"</div>"
							}
						}), Sa.fixedHeader && (i = "mbsc-lv-fixed-header" + (jt ?
							" mbsc-lv-fixed-header-ctx mbsc-lv-" + Sa.theme + (Sa.baseTheme ?
								" mbsc-lv-" + Sa.baseTheme : "") : ""), Et ? Et.attr("class", i) : (Et =
							r('<div class="' + i + '"></div>'), jt ? Ba.before(Et) : pt.prepend(Et),
							ha = c.throttle(L, 200), Ba.on("scroll touchmove", ha))), Sa.hover && (Bt || pt
							.on("mouseover.mbsc-lv", ".mbsc-lv-item", function() {
								Jt && Jt[0] == this || (N(), Jt = r(this), $a[Jt.attr("data-type") ||
									"defaults"].actions && (Ut = setTimeout(function() {
									pa ? Jt = null : (Rt = !0, Ga.openActions(Jt, $t,
										Bt, !1))
								}, qt)))
							}).on("mouseleave.mbsc-lv", N), Bt = Sa.hover.time || 200, qt = Sa.hover
							.timeout || 200, $t = Sa.hover.direction || Sa.hover || "right"), Za.is(
							"[mbsc-enhance]") && (Yt = !0, Za.removeAttr("mbsc-enhance"), pt.attr(
							"mbsc-enhance", "")), pt.trigger("mbsc-enhance", [{
							theme: Sa.theme,
							lang: Sa.lang
						}]), Ht("onInit")
				}, Ga.destroy = function() {
					xt.append(vt), jt && Et && Et.remove(), Yt && Za.attr("mbsc-enhance", ""), pt.find(
							".mbsc-lv-txt,.mbsc-lv-img").removeClass("mbsc-lv-txt mbsc-lv-img"), pt.find(
							"ul,ol").removeClass("mbsc-lv mbsc-lv-v mbsc-lv-root mbsc-lv-sl-curr").find(
							"li").removeClass(
							"mbsc-lv-gr-title mbsc-lv-item mbsc-lv-item-enhanced mbsc-lv-parent mbsc-lv-img-left mbsc-lv-img-right mbsc-lv-item-ic-left mbsc-lv-item-ic-right"
							).removeAttr("data-ref"), r(
							".mbsc-lv-back,.mbsc-lv-handle-c,.mbsc-lv-arr,.mbsc-lv-item-ic", pt).remove(),
						Za.insertAfter(pt), pt.remove(), Qt.remove(), Ba.off("scroll touchmove", ha).off(
							"orientationchange resize", F), Ga._destroy()
				};
				var is, rs = [],
					os = [],
					ls = [],
					cs = 0;
				Ga.startActionTrack = function() {
					cs || (ls = []), cs++
				}, Ga.endActionTrack = function() {
					cs--, cs || os.push(ls)
				}, Ga.addUndoAction = function(e, t) {
					var a = {
						action: e,
						async: t
					};
					cs ? ls.push(a) : (os.push([a]), os.length > Sa.undoLimit && os.shift())
				}, Ga.undo = function() {
					function e() {
						0 > s ? (is = !1, t()) : (a = n[s], s--, a.async ? a.action(e) : (a.action(), e()))
					}

					function t() {
						n = rs.shift(), n && (is = !0, s = n.length - 1, e())
					}
					var a, s, n;
					os.length && rs.push(os.pop()), is || t()
				}, Sa = Ga.settings, Ht = Ga.trigger, Ga.init(y)
			}, l.ListView.prototype = {
				_class: "listview",
				_hasDef: !0,
				_hasTheme: !0,
				_hasLang: !0,
				_defaults: {
					context: "body",
					actionsWidth: 90,
					sortDelay: 250,
					undoLimit: 10,
					swipe: !0,
					quickSwipe: !0,
					animateIcons: !0,
					animation: !0,
					revert: !0,
					vibrate: !0,
					handleClass: "",
					handleMarkup: '<div class="mbsc-lv-handle-bar mbsc-lv-handle"></div><div class="mbsc-lv-handle-bar mbsc-lv-handle"></div><div class="mbsc-lv-handle-bar mbsc-lv-handle"></div>',
					leftArrowClass: "mbsc-ic-arrow-left4",
					rightArrowClass: "mbsc-ic-arrow-right4",
					backText: "Back",
					undoText: "Undo",
					stages: []
				}
			}, i.themes.listview.mobiscroll = {
				leftArrowClass: "mbsc-ic-arrow-left5",
				rightArrowClass: "mbsc-ic-arrow-right5"
			}, i.presetShort("listview", "ListView")
		}(window, document),
		function() {
			t.themes.listview.ios = {
				leftArrowClass: "mbsc-ic-ion-ios7-arrow-back",
				rightArrowClass: "mbsc-ic-ion-ios7-arrow-forward"
			}
		}(),
		function() {
			t.themes.listview.jqm = {
				handleClass: "ui-btn ui-icon-bars ui-btn-up-c ui-btn-icon-notext ui-icon-shadow ui-corner-all ui-btn-corner-all",
				handleMarkup: '<span class="ui-btn-inner mbsc-lv-handle"><span class="ui-icon ui-icon-bars ui-icon-shadow mbsc-lv-handle">&nbsp;</span></span>',
				leftArrowClass: "ui-btn-icon-left ui-icon-carat-l",
				rightArrowClass: "ui-btn-icon-right ui-icon-carat-r",
				onInit: function() {
					$(this).closest(".mbsc-lv-cont").addClass($(this).data("inset") ? "mbsc-lv-jqm-inset" :
						"").find(".mbsc-lv-dummy, .mbsc-lv-fixed-header").addClass("ui-listview"), $(
						"ul,ol", this).listview("refresh")
				},
				onItemAdd: function(e) {
					var t = $(e.target).parent();
					t.hasClass("ui-listview") ? t.listview("refresh") : t.listview()
				},
				onSortUpdate: function(e) {
					$(e.target).parent().listview("refresh")
				}
			}
		}(),
		function() {
			var e = t,
				a = e.$;
			e.themes.listview.material = {
				leftArrowClass: "mbsc-ic-material-keyboard-arrow-left",
				rightArrowClass: "mbsc-ic-material-keyboard-arrow-right",
				onItemActivate: function(t) {
					e.themes.material.addRipple(a(t.target), t.domEvent)
				},
				onItemDeactivate: function() {
					e.themes.material.removeRipple()
				},
				onSlideStart: function(e) {
					a(".mbsc-ripple", e.target).remove()
				},
				onSortStart: function(e) {
					a(".mbsc-ripple", e.target).remove()
				}
			}
		}(),
		function(e, a, s) {
			var n = t,
				i = n.$,
				r = i.extend,
				o = n.classes;
			o.MenuStrip = function(t, a) {
				function l(e) {
					clearTimeout(S), S = setTimeout(function() {
						f("load" !== e.type)
					}, 200)
				}

				function c(e, t) {
					if (e.length) {
						var a = e.offset().left,
							n = e[0].offsetLeft,
							r = e[0].offsetWidth,
							o = b.offset().left;
						p = e, t === s && (t = !C), D && t && (C ? e.attr("data-selected") ? d(e) : m(e) : (d(i(
								".mbsc-ms-item-sel", H)), m(e))), "a" == A ? o > a ? _.scroll(-n, L, !0) : a +
							r > o + x && _.scroll(x - n - r, L, !0) : _.scroll(x / 2 - n - r / 2, L, !0), t &&
							F("onItemTap", {
								target: e[0]
							})
					}
				}

				function m(e) {
					e.addClass(k).attr("data-selected", "true").attr("aria-selected", "true")
				}

				function d(e) {
					e.removeClass(k).removeAttr("data-selected").removeAttr("aria-selected")
				}

				function u(e) {
					return "object" != typeof e && (e = H.children('[data-id="' + e + '"]')), i(e)
				}

				function h() {
					F("onMarkupInit"), H.children().each(function(e) {
						var t, a, s = i(this),
							n = D && "true" == s.attr("data-selected"),
							r = "true" == s.attr("data-disabled"),
							o = s.attr("data-icon");
						0 === e && (v = s), D && !C && n && (p = s), 1 !== s.children().length && i(
							"<span></span>").append(s.contents()).appendTo(s), a = s.children().eq(
							0), o && (T = !0), a.hasClass("mbsc-ms-item-i") || (a.html() && (y = !
							0), t = i(
								'<span class="mbsc-ms-item-i-t"><span class="mbsc-ms-item-i-c"></span></span>'
								), t.find(".mbsc-ms-item-i-c").append(a.contents()), a.addClass(
								"mbsc-ms-item-i" + (o ? " mbsc-ms-ic mbsc-ic mbsc-ic-" + o : ""))
							.append(t), s.attr("data-role", "button").attr("aria-selected", n ?
								"true" : null).attr("aria-disabled", r ? "true" : null).addClass(
								"mbsc-ms-item mbsc-btn-e " + (V.itemClass || "") + (n ? k : "") + (
									r ? " mbsc-btn-d " + (V.disabledClass || "") : "")))
					}), T && b.addClass("mbsc-ms-icons"), y && b.addClass("mbsc-ms-txt")
				}

				function f(e, t) {
					var a = V.itemWidth,
						s = V.layout;
					I.contWidth = x = b.width(), e && M === x || !x || (M = x, n.util.isNumeric(s) && (w = x ?
							x / s : a, a > w && (s = "liquid")), a && ("liquid" == s ? w = x ? x / Math.min(
							Math.floor(x / a), H.children().length) : a : "fixed" == s && (w = a)), w && H
						.children().css("width", w + "px"), H.contents().filter(function() {
							return 3 == this.nodeType && !/\S/.test(this.nodeValue)
						}).remove(), I.totalWidth = N = H.width(), r(_.settings, {
							contSize: x,
							maxSnapScroll: V.paging ? 1 : !1,
							maxScroll: 0,
							minScroll: N > x ? x - N : 0,
							snap: V.paging ? x : V.snap ? w || ".mbsc-ms-item" : !1,
							elastic: N > x ? w || x : !1
						}), _.refresh(t))
				}
				var p, b, v, g, x, T, y, w, C, M, S, D, k, _, A, V, N, F, L = 1e3,
					I = this,
					H = i(t);
				o.Base.call(this, t, a, !0), I.navigate = function(e, t) {
					c(u(e), t)
				}, I.next = function(e) {
					var t = p ? p.next() : v;
					t.length && (p = t, c(p, e))
				}, I.prev = function(e) {
					var t = p ? p.prev() : v;
					t.length && (p = t, c(p, e))
				}, I.select = function(e) {
					C || d(i(".mbsc-ms-item-sel", H)), m(u(e))
				}, I.deselect = function(e) {
					d(u(e))
				}, I.enable = function(e) {
					u(e).removeClass("mbsc-btn-d").removeAttr("data-disabled").removeAttr("aria-disabled")
				}, I.disable = function(e) {
					u(e).addClass("mbsc-btn-d").attr("data-disabled", "true").attr("aria-disabled", "true")
				}, I.refresh = I.position = function(e) {
					H.height(""), h(), f(!1, e), H.height(H.height())
				}, I.init = function(t) {
					var a;
					I._init(t), g = i("body" == V.context ? e : V.context), "tabs" == V.type ? (V.select = V
							.select || "single", V.variant = V.variant || "b") : "options" == V.type ? (V
							.select = V.select || "multiple", V.variant = V.variant || "a") : "menu" == V
						.type && (V.select = V.select || "off", V.variant = V.variant || "a"), V
						.itemWidth && V.snap === s && (V.snap = !0), A = V.variant, D = "off" != V.select,
						C = "multiple" == V.select, k = " mbsc-ms-item-sel " + (V.activeClass || ""), a =
						"mbsc-ms-c mbsc-ms-" + A + " mbsc-ms-" + V.display + " mbsc-" + V.theme + " " + (V
							.baseTheme ? " mbsc-" + V.baseTheme : "") + " " + (V.cssClass || "") + " " + (V
							.wrapperClass || "") + (V.rtl ? " mbsc-ms-rtl" : " mbsc-ms-ltr") + (V
							.itemWidth ? " mbsc-ms-hasw" : "") + ("body" == V.context ? "" :
						" mbsc-ms-ctx") + (D ? "" : " mbsc-ms-nosel"), b ? (H.height(""), b.attr("class",
							a)) : (b = i('<div class="' + a + '"><div class="mbsc-ms-sc"></div></div>')
							.insertAfter(H), b.find(".mbsc-ms-sc").append(H), _ = new n.classes.ScrollView(
								b[0], {
									axis: "X",
									contSize: 0,
									maxScroll: 0,
									maxSnapScroll: 1,
									minScroll: 0,
									snap: 1,
									elastic: 1,
									rtl: V.rtl,
									mousewheel: V.mousewheel,
									onBtnTap: function(e) {
										c(i(e.target), !0)
									},
									onGestureStart: function(e) {
										F("onGestureStart", e)
									},
									onGestureEnd: function(e) {
										F("onGestureEnd", e)
									},
									onMove: function(e) {
										F("onMove", e)
									},
									onAnimationStart: function(e) {
										F("onAnimationStart", e)
									},
									onAnimationEnd: function(e) {
										F("onAnimationEnd", e)
									}
								})), H.css("display", "").addClass("mbsc-ms " + (V.groupClass || "")), h(),
						F("onMarkupReady", {
							target: b[0]
						}), H.height(H.height()), f(), b.find("img").on("load", l), g.on(
							"orientationchange resize", l), F("onInit")
				}, I.destroy = function() {
					g.off("orientationchange resize", l), H.height("").insertAfter(b).find(".mbsc-ms-item")
						.width(""), b.remove(), _.destroy(), I._destroy()
				}, V = I.settings, F = I.trigger, I.init(a)
			}, o.MenuStrip.prototype = {
				_class: "menustrip",
				_hasDef: !0,
				_hasTheme: !0,
				_defaults: {
					context: "body",
					type: "options",
					display: "inline",
					layout: "liquid"
				}
			}, n.presetShort("menustrip", "MenuStrip")
		}(window, document),
		function() {
			t.themes.menustrip["android-holo"] = {}
		}(),
		function() {
			t.themes.menustrip.bootstrap = {
				wrapperClass: "popover panel panel-default",
				groupClass: "btn-group",
				activeClass: "btn-primary",
				disabledClass: "disabled",
				itemClass: "btn btn-default"
			}
		}(),
		function() {
			t.themes.menustrip.ios = {}
		}(),
		function() {
			var e = t.$,
				a = e.mobile && e.mobile.version && e.mobile.version.match(/1\.4/);
			t.themes.menustrip.jqm = {
				activeClass: "ui-btn-active",
				disabledClass: "ui-state-disabled",
				onThemeLoad: function(e) {
					var t = e.settings,
						s = t.jqmSwatch || (a ? "a" : "c");
					t.itemClass = "ui-btn ui-btn-up-" + s, t.wrapperClass = "ui-bar-" + s
				}
			}
		}(),
		function() {
			var e = t.$;
			t.themes.menustrip.material = {
				onInit: function() {
					t.themes.material.initRipple(e(this), ".mbsc-ms-item", "mbsc-btn-d", "mbsc-btn-nhl")
				},
				onMarkupInit: function() {
					e(".mbsc-ripple", this).remove()
				}
			}
		}(),
		function() {
			t.themes.menustrip.wp = {}
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = {
					batch: 50,
					min: 0,
					max: 100,
					defaultUnit: "",
					units: null,
					unitNames: null,
					invalid: [],
					sign: !1,
					step: .05,
					scale: 2,
					convert: function(e) {
						return e
					},
					signText: "&nbsp;",
					wholeText: "Whole",
					fractionText: "Fraction",
					unitText: "Unit"
				};
			a.presets.scroller.measurement = function(t) {
				function a(e) {
					return Math.max(w, Math.min(C, z ? 0 > e ? Math.ceil(e) : Math.floor(e) : l(Math.round(e -
						B), J) + B))
				}

				function i(e) {
					return z ? l((Math.abs(e) - Math.abs(a(e))) * $ - q, J) + q : 0
				}

				function r(e) {
					var t = a(e),
						s = i(e),
						n = 0 > e ? "-" : "+";
					return s >= $ && (0 > e ? t-- : t++, s = 0), [n, t, s]
				}

				function o(e) {
					var t = +e[g],
						a = z ? e[v] / $ * (0 > t ? -1 : 1) : 0;
					return (O && "-" == e[0] ? -1 : 1) * (t + a)
				}

				function l(e, t) {
					return Math.round(e / t) * t
				}

				function c(e, t) {
					for (e += ""; e.length < t;) e = "0" + e;
					return e
				}

				function m(e, t, a) {
					return t !== a && A.convert ? A.convert.call(this, e, t, a) : e
				}

				function d(e, t, a) {
					return e = e > a ? a : e, e = t > e ? t : e
				}

				function u(e) {
					var t, a;
					T = m(A.min, E, e), y = m(A.max, E, e), z ? (w = 0 > T ? Math.ceil(T) : Math.floor(T), C =
							0 > y ? Math.ceil(y) : Math.floor(y), M = i(T), S = i(y)) : (w = Math.round(T), C =
							Math.round(y), C = w + Math.floor((C - w) / J) * J, B = w % J), t = w, a = C, O && (
							a = Math.abs(Math.abs(t) > Math.abs(a) ? t : a), t = 0 > t ? 0 : t), L.min = 0 > t ?
						Math.ceil(t / j) : Math.floor(t / j), L.max = 0 > a ? Math.ceil(a / j) : Math.floor(a /
							j)
				}

				function h(e) {
					return o(e).toFixed(z ? W : 0) + (P ? " " + Y[e[x]] : "")
				}
				var f, p, b, v, g, x, T, y, w, C, M, S, D, k, _ = s.extend({}, t.settings),
					A = s.extend(t.settings, n, _),
					V = {},
					N = [
						[]
					],
					F = {},
					L = {},
					I = {},
					H = [],
					O = A.sign,
					P = A.units && A.units.length,
					E = P ? A.defaultUnit || A.units[0] : "",
					Y = [],
					z = A.step < 1,
					j = A.step > 1 ? A.step : 1,
					W = z ? Math.max(A.scale, (A.step + "").split(".")[1].length) : 1,
					$ = Math.pow(10, W),
					J = Math.round(z ? A.step * $ : A.step),
					R = -1,
					B = 0,
					q = 0,
					U = 0;
				if (t.setVal = function(e, a, n, i, r) {
						t._setVal(s.isArray(e) ? h(e) : e, a, n, i, r)
					}, A.units)
					for (k = 0; k < A.units.length; ++k) D = A.units[k], Y.push(A.unitNames ? A.unitNames[D] ||
						D : D);
				if (O)
					if (O = !1, P)
						for (k = 0; k < A.units.length; k++) m(A.min, E, A.units[k]) < 0 && (O = !0);
					else O = A.min < 0;
				if (O && (N[0].push({
						data: ["-", "+"],
						label: A.signText
					}), R = U++), L = {
						label: A.wholeText,
						data: function(e) {
							return w % j + e * j
						},
						getIndex: function(e) {
							return Math.round((e - w % j) / j)
						}
					}, N[0].push(L), g = U++, u(E), z) {
					for (N[0].push(I), I.data = [], I.label = A.fractionText, k = q; $ > k; k += J) H.push(k), I
						.data.push({
							value: k,
							display: "." + c(k, W)
						});
					v = U++, f = Math.ceil(100 / J), A.invalid && A.invalid.length && (s.each(A.invalid,
						function(e, t) {
							var a = t > 0 ? Math.floor(t) : Math.ceil(t);
							0 === a && (a = 0 >= t ? -.001 : .001), F[a] = (F[a] || 0) + 1, 0 === t && (
								a = .001, F[a] = (F[a] || 0) + 1)
						}), s.each(F, function(e, t) {
						f > t ? delete F[e] : F[e] = e
					}))
				}
				if (P) {
					for (V = {
							data: [],
							label: A.unitText,
							circular: !1
						}, k = 0; k < A.units.length; k++) V.data.push({
						value: k,
						display: Y[k]
					});
					N[0].push(V)
				}
				return x = U, {
					wheels: N,
					minWidth: O && z ? 70 : 80,
					showLabel: !1,
					formatValue: h,
					parseValue: function(e) {
						var t, a = ("number" == typeof e ? e + "" : e) || A.defaultValue,
							n = (a + "").split(" "),
							i = +n[0],
							o = [],
							l = "";
						return P && (l = s.inArray(n[1], Y), l = -1 == l ? s.inArray(E, A.units) : l,
								l = -1 == l ? 0 : l), b = P ? A.units[l] : "", u(b), i = isNaN(i) ? 0 :
							i, i = d(i, T, y), t = r(i), t[1] = d(t[1], w, C), p = i, O && (o[0] = t[0],
								t[1] = Math.abs(t[1])), o[g] = t[1], z && (o[v] = t[2]), P && (o[x] =
							l), o
					},
					onCancel: function() {
						p = e
					},
					validate: function(a) {
						var n, i, c, h, f, D = a.values,
							k = a.index,
							_ = a.direction,
							V = {},
							N = [],
							I = {},
							Y = P ? A.units[D[x]] : "";
						if (O && 0 === k && (p = Math.abs(p) * ("-" == D[0] ? -1 : 1)), (k === g ||
								k === v && z || p === e || k === e) && (p = o(D), b = Y), (P && k ===
								x && b !== Y || k === e) && (u(Y), p = m(p, b, Y), b = Y, i = r(p),
								k !== e && (I[g] = L, t.changeWheel(I)), O && (D[0] = i[0])), N[g] = [],
							O)
							for (N[0] = [], T > 0 && (N[0].push("-"), D[0] = "+"), 0 > y && (N[0].push(
									"+"), D[0] = "-"), f = Math.abs("-" == D[0] ? w : C), U = f + j; f +
								20 * j > U; U += j) N[g].push(U), V[U] = !0;
						if (p = d(p, T, y), i = r(p), c = O ? Math.abs(i[1]) : i[1], n = O ? "-" == D[
							0] : 0 > p, D[g] = c, n && (i[0] = "-"), z && (D[v] = i[2]), s.each(z ? F :
								A.invalid,
								function(e, t) {
									if (O && n) {
										if (!(0 >= t)) return;
										t = Math.abs(t)
									}
									t = l(m(t, E, Y), z ? 1 : J), V[t] = !0, N[g].push(t)
								}), D[g] = t.getValidValue(g, c, _, V), i[1] = D[g] * (O && n ? -1 : 1),
							z) {
							N[v] = [];
							var W = O ? D[0] + D[1] : (0 > p ? "-" : "+") + Math.abs(i[1]),
								$ = (0 > T ? "-" : "+") + Math.abs(w),
								R = (0 > y ? "-" : "+") + Math.abs(C);
							W === $ && s(H).each(function(e, t) {
								(n ? t > M : M > t) && N[v].push(t)
							}), W === R && s(H).each(function(e, t) {
								(n ? S > t : t > S) && N[v].push(t)
							}), s.each(A.invalid, function(e, t) {
								h = r(m(t, E, Y)), (i[0] === h[0] || 0 === i[1] && 0 === h[1] &&
									0 === h[2]) && i[1] === h[1] && N[v].push(h[2])
							})
						}
						return {
							disabled: N,
							valid: D
						}
					}
				}
			}, a.presetShort("measurement")
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.scroller,
				n = {
					min: 0,
					max: 100,
					defaultUnit: "km",
					units: ["m", "km", "in", "ft", "yd", "mi"]
				},
				i = {
					mm: .001,
					cm: .01,
					dm: .1,
					m: 1,
					dam: 10,
					hm: 100,
					km: 1e3,
					"in": .0254,
					ft: .3048,
					yd: .9144,
					ch: 20.1168,
					fur: 201.168,
					mi: 1609.344,
					lea: 4828.032
				};
			e.presetShort("distance"), s.distance = function(e) {
				var t = a.extend({}, n, e.settings);
				return a.extend(e.settings, t, {
					sign: !1,
					convert: function(e, t, a) {
						return e * i[t] / i[a]
					}
				}), s.measurement.call(this, e)
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.scroller,
				n = {
					min: 0,
					max: 100,
					defaultUnit: "N",
					units: ["N", "kp", "lbf", "pdl"]
				},
				i = {
					N: 1,
					kp: 9.80665,
					lbf: 4.448222,
					pdl: .138255
				};
			e.presetShort("force"), s.force = function(e) {
				var t = a.extend({}, n, e.settings);
				return a.extend(e.settings, t, {
					sign: !1,
					convert: function(e, t, a) {
						return e * i[t] / i[a]
					}
				}), s.measurement.call(this, e)
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.scroller,
				n = {
					min: 0,
					max: 1e3,
					defaultUnit: "kg",
					units: ["g", "kg", "oz", "lb"],
					unitNames: {
						tlong: "t (long)",
						tshort: "t (short)"
					}
				},
				i = {
					mg: .001,
					cg: .01,
					dg: .1,
					g: 1,
					dag: 10,
					hg: 100,
					kg: 1e3,
					t: 1e6,
					drc: 1.7718452,
					oz: 28.3495,
					lb: 453.59237,
					st: 6350.29318,
					qtr: 12700.58636,
					cwt: 50802.34544,
					tlong: 1016046.9088,
					tshort: 907184.74
				};
			e.presetShort("mass"), s.mass = function(e) {
				var t = a.extend({}, n, e.settings);
				return a.extend(e.settings, t, {
					sign: !1,
					convert: function(e, t, a) {
						return e * i[t] / i[a]
					}
				}), s.measurement.call(this, e)
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.scroller,
				n = {
					min: 0,
					max: 100,
					defaultUnit: "kph",
					units: ["kph", "mph", "mps", "fps", "knot"],
					unitNames: {
						kph: "km/h",
						mph: "mi/h",
						mps: "m/s",
						fps: "ft/s",
						knot: "knot"
					}
				},
				i = {
					kph: 1,
					mph: 1.60934,
					mps: 3.6,
					fps: 1.09728,
					knot: 1.852
				};
			e.presetShort("speed"), s.speed = function(e) {
				var t = a.extend({}, n, e.settings);
				return a.extend(e.settings, t, {
					sign: !1,
					convert: function(e, t, a) {
						return e * i[t] / i[a]
					}
				}), s.measurement.call(this, e)
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.presets.scroller,
				n = {
					min: -20,
					max: 40,
					defaultUnit: "c",
					units: ["c", "k", "f", "r"],
					unitNames: {
						c: "°C",
						k: "K",
						f: "°F",
						r: "°R"
					}
				},
				i = {
					c2k: function(e) {
						return e + 273.15
					},
					c2f: function(e) {
						return 9 * e / 5 + 32
					},
					c2r: function(e) {
						return 9 * (e + 273.15) / 5
					},
					k2c: function(e) {
						return e - 273.15
					},
					k2f: function(e) {
						return 9 * e / 5 - 459.67
					},
					k2r: function(e) {
						return 9 * e / 5
					},
					f2c: function(e) {
						return 5 * (e - 32) / 9
					},
					f2k: function(e) {
						return 5 * (e + 459.67) / 9
					},
					f2r: function(e) {
						return e + 459.67
					},
					r2c: function(e) {
						return 5 * (e - 491.67) / 9
					},
					r2k: function(e) {
						return 5 * e / 9
					},
					r2f: function(e) {
						return e - 459.67
					}
				};
			e.presetShort("temperature"), s.temperature = function(e) {
				var t = a.extend({}, n, e.settings);
				return a.extend(e.settings, t, {
					sign: !0,
					convert: function(e, t, a) {
						return i[t + "2" + a](e)
					}
				}), s.measurement.call(this, e)
			}
		}(),
		function() {
			var e = t,
				a = e.presets.scroller;
			a.number = a.measurement, e.presetShort("number")
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = a.presets.scroller,
				i = a.util.datetime,
				r = a.util,
				o = r.testTouch,
				l = {
					autoCorrect: !0,
					showSelector: !0,
					minRange: 1,
					rangeTap: !0,
					fromText: "Start",
					toText: "End"
				};
			a.presetShort("range"), n.range = function(t) {
				function a(e, t) {
					e && (e.setFullYear(t.getFullYear()), e.setMonth(t.getMonth()), e.setDate(t.getDate()))
				}

				function r(e) {
					t._startDate = A = C, t._endDate = V = M, I.startInput && (s(I.startInput).val(y), e && s(I
						.startInput).trigger("change")), I.endInput && (s(I.endInput).val(w), e && s(I
						.endInput).trigger("change"))
				}

				function c(e, t) {
					var a = !0;
					return e && C && M && (M - C > I.maxRange - 1 && (N ? C = new Date(M - I.maxRange + 1) : M =
							new Date(+C + I.maxRange - 1)), M - C < I.minRange - 1 && (N ? C = new Date(M -
							I.minRange + 1) : M = new Date(+C + I.minRange - 1))), C && M || (a = !1), t && h(),
						a
				}

				function m() {
					return C && M ? Math.max(1, Math.round((new Date(M).setHours(0, 0, 0, 0) - new Date(C)
						.setHours(0, 0, 0, 0)) / 864e5) + 1) : 0
				}

				function d(e) {
					e.addClass("mbsc-range-btn-sel").attr("aria-checked", "true").find(".mbsc-range-btn")
						.addClass(P)
				}

				function u() {
					_ && p && (s(".mbsc-range-btn-c", p).removeClass("mbsc-range-btn-sel").removeAttr(
						"aria-checked").find(".mbsc-range-btn", p).removeClass(P), d(s(
						".mbsc-range-btn-c", p).eq(N)))
				}

				function h() {
					var e, t, a, n, r, o = 0,
						l = O || !N ? " mbsc-cal-day-hl mbsc-cal-sel-start" : " mbsc-cal-sel-start",
						c = O || N ? " mbsc-cal-day-hl mbsc-cal-sel-end" : " mbsc-cal-sel-end";
					if (y = C ? i.formatDate(v, C, I) : "", w = M ? i.formatDate(v, M, I) : "", p && (s(
								".mbsc-range-btn-v-start", p).html(y || "&nbsp;"), s(".mbsc-range-btn-v-end", p)
							.html(w || "&nbsp;"), e = C ? new Date(C) : null, a = M ? new Date(M) : null, !e &&
							a && (e = new Date(a)), !a && e && (a = new Date(e)), r = N ? a : e, s(
								".mbsc-cal-table .mbsc-cal-day-sel .mbsc-cal-day-i", p).removeClass(P), s(
								".mbsc-cal-table .mbsc-cal-day-hl", p).removeClass(Y), s(
								".mbsc-cal-table .mbsc-cal-day-sel", p).removeClass(
								"mbsc-cal-day-sel mbsc-cal-sel-start mbsc-cal-sel-end").removeAttr(
								"aria-selected"), e && a))
						for (t = e.setHours(0, 0, 0, 0), n = a.setHours(0, 0, 0, 0); a >= e && 84 > o;) s(
							'.mbsc-cal-day[data-full="' + r.getFullYear() + "-" + r.getMonth() + "-" + r
							.getDate() + '"]', p).addClass("mbsc-cal-day-sel" + (r.getTime() === t ? l :
							"") + (r.getTime() === n ? c : "")).attr("aria-selected", "true").find(
							".mbsc-cal-day-i ").addClass(P), r.setDate(r.getDate() + (N ? -1 : 1)), o++
				}
				var f, p, b, v, g, x, T, y, w, C, M, S, D, k, _, A = t._startDate,
					V = t._endDate,
					N = 0,
					F = new Date,
					L = s.extend({}, t.settings),
					I = s.extend(t.settings, l, L),
					H = I.anchor,
					O = I.rangeTap,
					P = I.activeClass || "",
					E = "mbsc-fr-btn-d " + (I.disabledClass || ""),
					Y = "mbsc-cal-day-hl",
					z = null === I.defaultValue ? [] : I.defaultValue || [new Date(F.setHours(0, 0, 0, 0)),
						new Date(F.getFullYear(), F.getMonth(), F.getDate() + 6, 23, 59, 59, 999)
					];
				return O && (I.tabs = !0), g = n.calbase.call(this, t), f = s.extend({}, g), v = t.format, S =
					"time" === I.controls.join(""), _ = 1 == I.controls.length && "calendar" == I.controls[0] ?
					I.showSelector : !0, I.startInput && (D = s(I.startInput).prop("readonly"), t.attachShow(s(I
						.startInput).prop("readonly", !0), function() {
						N = 0, I.anchor = H || s(I.startInput)
					})), I.endInput && (k = s(I.endInput).prop("readonly"), t.attachShow(s(I.endInput).prop(
						"readonly", !0), function() {
						N = 1, I.anchor = H || s(I.endInput)
					})), t.setVal = function(a, s, n, r, o) {
						var l = a || [],
							c = a;
						(l[0] === e || null === l[0] || l[0].getTime) && (T = !0, C = l[0] || null, y = C ? i
							.formatDate(v, C, I) : "", N || (c = f.parseValue(y, t))), (l[1] === e || null ===
							l[1] || l[1].getTime) && (T = !0, M = l[1] || null, w = M ? i.formatDate(v, M, I) :
							"", N && (c = f.parseValue(w, t))), r || (t._startDate = A = C, t._endDate = V = M),
							t._setVal(c, s, n, r, o)
					}, t.getVal = function(e) {
						return e ? [C, M] : t._hasValue ? [A, V] : null
					}, t.getDayProps = function(e) {
						var t = C ? new Date(C.getFullYear(), C.getMonth(), C.getDate()) : null,
							a = M ? new Date(M.getFullYear(), M.getMonth(), M.getDate()) : null;
						return {
							selected: t && a && e >= t && M >= e,
							cssClass: ((O || !N) && t && t.getTime() === e.getTime() || (O || N) && a && a
								.getTime() === e.getTime() ? Y : "") + (t && t.getTime() === e.getTime() ?
								" mbsc-cal-sel-start" : "") + (a && a.getTime() === e.getTime() ?
								" mbsc-cal-sel-end" : "")
						}
					}, t.setActiveDate = function(e) {
						var a;
						N = "start" == e ? 0 : 1, a = "start" == e ? C : M, t.isVisible() && (u(), O || (s(
							".mbsc-cal-table .mbsc-cal-day-hl", p).removeClass(Y), a && s(
							'.mbsc-cal-day[data-full="' + a.getFullYear() + "-" + a.getMonth() +
							"-" + a.getDate() + '"]', p).addClass(Y)), a && (x = !0, t.setDate(a, !1,
							1e3, !0)))
					}, t.getValue = t.getVal, s.extend(g, {
						highlight: !1,
						outerMonthChange: !1,
						formatValue: function() {
							return y + (I.endInput ? "" : w ? " - " + w : "")
						},
						parseValue: function(e) {
							var a = e ? e.split(" - ") : [];
							return I.defaultValue = z[1], V = I.endInput ? s(I.endInput).val() ? i
								.parseDate(v, s(I.endInput).val(), I) : z[1] : a[1] ? i.parseDate(v, a[
									1], I) : z[1], I.defaultValue = z[0], A = I.startInput ? s(I
									.startInput).val() ? i.parseDate(v, s(I.startInput).val(), I) : z[
								0] : a[0] ? i.parseDate(v, a[0], I) : z[0], I.defaultValue = z[N], y =
								A ? i.formatDate(v, A, I) : "", w = V ? i.formatDate(v, V, I) : "", t
								._startDate = A, t._endDate = V, f.parseValue(N ? w : y, t)
						},
						onFill: function(e) {
							r(e.change)
						},
						onBeforeClose: function(e) {
							return "set" !== e.button || c(!0, !0) ? void 0 : (t.setActiveDate(N ?
								"start" : "end"), !1)
						},
						onHide: function() {
							f.onHide.call(t), N = 0, p = null, I.anchor = H
						},
						onClear: function() {
							O && (N = 0)
						},
						onBeforeShow: function() {
							I.headerText = !1, C = A, M = V, I.counter && (I.headerText = function() {
								var e = m();
								return (e > 1 ? I.selectedPluralText || I.selectedText : I
									.selectedText).replace(/{count}/, e)
							}), T = !0
						},
						onMarkupReady: function(e) {
							var a;
							p = s(e.target), C && (x = !0, t.setDate(C, !1, 0, !0), C = t.getDate(!0)),
								M && (x = !0, t.setDate(M, !1, 0, !0), M = t.getDate(!0)), (N && M || !
									N && C) && (x = !0, t.setDate(N ? M : C, !1, 0, !0)), f
								.onMarkupReady.call(this, e), p.addClass("mbsc-range"), _ && (a =
									'<div class="mbsc-range-btn-t" role="radiogroup"><div class="mbsc-range-btn-c mbsc-range-btn-start"><div role="radio" class="mbsc-fr-btn-e mbsc-fr-btn-nhl mbsc-range-btn">' +
									I.fromText +
									'<div class="mbsc-range-btn-v mbsc-range-btn-v-start">' + (y ||
										"&nbsp;") +
									'</div></div></div><div class="mbsc-range-btn-c mbsc-range-btn-end"><div role="radio" class="mbsc-fr-btn-e mbsc-fr-btn-nhl mbsc-range-btn">' +
									I.toText + '<div class="mbsc-range-btn-v mbsc-range-btn-v-end">' + (
										w || "&nbsp;") + "</div></div></div></div>", s(".mbsc-cal-tabs",
										p).before(a), u()), s(".mbsc-range-btn-c", p).on(
									"touchstart click",
									function(e) {
										o(e, this) && (t.showMonthView(), t.setActiveDate(s(this)
										.index() ? "end" : "start"))
									})
						},
						onDayChange: function(e) {
							e.active = N ? "end" : "start", b = !0
						},
						onSetDate: function(n) {
							var i = n.date,
								r = t.order;
							x || (r.h === e && i.setHours(N ? 23 : 0), r.i === e && i.setMinutes(N ?
										59 : 0), r.s === e && i.setSeconds(N ? 59 : 0), i
									.setMilliseconds(N ? 999 : 0), (!T || b) && (O && b && (1 == N &&
										C > i && (N = 0), N ? i.setHours(23, 59, 59, 999) : i
										.setHours(0, 0, 0, 0)), N ? M = new Date(i) : C = new Date(
										i), S && (a(C, i), a(M, i)), O && b && !N && (M = null))), t
								._isValid = c(T || b || I.autoCorrect, !x), n.active = N ? "end" :
								"start", !x && O && (b && (N = N ? 0 : 1), u()), t.isVisible() && (t
									._isValid ? s(".mbsc-fr-btn-s .mbsc-fr-btn", t._markup).removeClass(
										E) : s(".mbsc-fr-btn-s .mbsc-fr-btn", t._markup).addClass(E)),
								b = !1, T = !1, x = !1
						},
						onTabChange: function(e) {
							"calendar" != e.tab && t.setDate(N ? M : C, !1, 1e3, !0), c(!0, !0)
						},
						onDestroy: function() {
							s(I.startInput).prop("readonly", D), s(I.endInput).prop("readonly", k)
						}
					}), g
			}
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = {
					inputClass: "",
					values: 5,
					order: "desc",
					style: "icon",
					invalid: [],
					icon: {
						filled: "star3",
						empty: "star3"
					}
				};
			a.presetShort("rating"), a.presets.scroller.rating = function(t) {
				var i, r, o, l, c, m, d, u, h, f, p = s.extend({}, t.settings),
					b = s.extend(t.settings, n, p),
					v = s(this),
					g = this.id + "_dummy",
					x = s('label[for="' + this.id + '"]').attr("for", g),
					T = b.label !== e ? b.label : x.length ? x.text() : v.attr("name"),
					y = b.defaultValue,
					w = [
						[]
					],
					C = {
						data: [],
						label: T,
						circular: !1
					},
					M = {},
					S = [],
					D = !1,
					k = "grade" === b.style ? "circle" : "icon";
				if (v.is("select") && (b.values = {}, s("option", v).each(function() {
						b.values[s(this).val()] = s(this).text()
					}), s("#" + g).remove()), s.isArray(b.values))
					for (r = 0; r < b.values.length; r++) u = +b.values[r], isNaN(u) && (u = r + 1, D = !0), S
						.push({
							order: u,
							key: b.values[r],
							value: b.values[r]
						});
				else if (s.isPlainObject(b.values)) {
					r = 1, D = !0;
					for (h in b.values) u = +h, isNaN(u) && (u = r), S.push({
						order: u,
						key: h,
						value: b.values[h]
					}), r++
				} else
					for (r = 1; r <= b.values; r++) S.push({
						order: r,
						key: r,
						value: r
					});
				for (b.showText === e && D && (b.showText = !0), b.icon.empty === e && (b.icon.empty = b.icon
						.filled), S.sort(function(e, t) {
						return "desc" == b.order ? t.order - e.order : e.order - t.order
					}), f = "desc" == b.order ? S[0].order : S[S.length - 1].order, r = 0; r < S.length; r++) {
					for (d = S[r].order, c = S[r].key, m = S[r].value, l = "", o = 1; d + 1 > o; o++) l +=
						'<span class="mbsc-rating-' + k + ("circle" === k ? "" : " mbsc-ic mbsc-ic-" + b.icon
							.filled) + ' ">' + ("circle" == k ? o : " ") + "</span>";
					for (o = d + 1; f >= o; o++) l += '<span class="mbsc-rating-' + k + ("circle" === k ?
						" mbsc-rating-circle-unf" : " mbsc-ic mbsc-ic-" + (b.icon.empty ? b.icon.empty +
							" mbsc-rating-icon-unf" : "") + (b.icon.empty === b.icon.filled ?
							" mbsc-rating-icon-same" : "")) + '"></span>';
					y === e && (y = c), l += b.showText ? '<span class="mbsc-rating-txt">' + m + "</span>" : "",
						C.data.push({
							value: c,
							display: l,
							label: m
						}), M[c] = m
				}
				return v.is("select") && (i = s('<input type="text" id="' + g + '" value="' + M[v.val()] +
						'" class="' + b.inputClass + '" placeholder="' + (b.placeholder || "") +
						'" readonly />').insertBefore(v)), w[0].push(C), i && t.attachShow(i), v.is("select") &&
					v.hide().closest(".ui-field-contain").trigger("create"), t.getVal = function(e) {
						var s = t._hasValue ? t[e ? "_tempWheelArray" : "_wheelArray"][0] : null;
						return a.util.isNumeric(s) ? +s : s
					}, {
						anchor: i,
						wheels: w,
						headerText: !1,
						compClass: "mbsc-rating",
						setOnTap: !0,
						formatValue: function(e) {
							return M[e[0]]
						},
						parseValue: function(e) {
							var t;
							for (t in M)
								if (i && t == e || !i && M[t] == e) return [t];
							return [y]
						},
						validate: function() {
							return {
								disabled: [b.invalid]
							}
						},
						onFill: function(e) {
							i && (i.val(e.valueText), v.val(t._tempWheelArray[0]))
						},
						onDestroy: function() {
							i && i.remove(), v.show()
						}
					}
			}
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = {
					autostart: !1,
					step: 1,
					useShortLabels: !1,
					labels: ["Years", "Months", "Days", "Hours", "Minutes", "Seconds", ""],
					labelsShort: ["Yrs", "Mths", "Days", "Hrs", "Mins", "Secs", ""],
					startText: "Start",
					stopText: "Stop",
					resetText: "Reset",
					lapText: "Lap",
					hideText: "Hide"
				};
			a.presetShort("timer"), a.presets.scroller.timer = function(t) {
				function a(e) {
					return new Date(e.getUTCFullYear(), e.getUTCMonth(), e.getUTCDate(), e.getUTCHours(), e
						.getUTCMinutes(), e.getUTCSeconds(), e.getUTCMilliseconds())
				}

				function i(e) {
					var t = {};
					if (E && k[H].index > k.days.index) {
						var n, i, r, o, l = new Date,
							c = x ? l : P,
							m = x ? P : l;
						for (m = a(m), c = a(c), t.years = c.getFullYear() - m.getFullYear(), t.months = c
							.getMonth() - m.getMonth(), t.days = c.getDate() - m.getDate(), t.hours = c
							.getHours() - m.getHours(), t.minutes = c.getMinutes() - m.getMinutes(), t.seconds =
							c.getSeconds() - m.getSeconds(), t.fract = (c.getMilliseconds() - m
							.getMilliseconds()) / 10, n = D.length; n > 0; n--) i = D[n - 1], r = k[i], o = D[s
							.inArray(i, D) - 1], k[o] && t[i] < 0 && (t[o]--, t[i] += "months" == o ? 32 -
							new Date(c.getFullYear(), c.getMonth(), 32).getDate() : r.until + 1);
						"months" == H && (t.months += 12 * t.years, delete t.years)
					} else s(D).each(function(a, s) {
						k[s].index <= k[H].index && (t[s] = Math.floor(e / k[s].limit), e -= t[s] * k[s]
							.limit)
					});
					return t
				}

				function r(e) {
					var t = 1,
						a = k[e],
						n = a.wheel,
						i = a.prefix,
						r = 0,
						l = a.until,
						c = k[D[s.inArray(e, D) - 1]];
					if (a.index <= k[H].index && (!c || c.limit > I))
						if (_[e] || Y[0].push(n), _[e] = 1, n.data = [], n.label = a.label || "", n.cssClass =
							"mbsc-timer-whl-" + e, I >= a.limit && (t = Math.max(Math.round(I / a.limit), 1),
								h = t * a.limit), e == H) n.min = 0, n.data = function(e) {
							return {
								value: e,
								display: o(e, i, a.label)
							}
						}, n.getIndex = function(e) {
							return e
						};
						else
							for (d = r; l >= d; d += t) n.data.push({
								value: d,
								display: o(d, i, a.label)
							})
				}

				function o(e, t, a) {
					return (t || "") + (10 > e ? "0" : "") + e + '<span class="mbsc-timer-lbl">' + a + "</span>"
				}

				function l(e) {
					var t, a = [],
						n = i(e);
					return s(D).each(function(e, s) {
						_[s] && (t = Math.max(Math.round(I / k[s].limit), 1), a.push(Math.round(n[s] /
							t) * t))
					}), a
				}

				function c(t) {
					E ? (v = P - new Date, 0 > v ? (v *= -1, x = !0) : x = !1, g = 0, L = !0) : P !== e ? (L = !
						1, v = 1e3 * P, x = "countdown" != C.mode, t && (g = 0)) : (v = 0, x =
						"countdown" != C.mode, L = x, t && (g = 0))
				}

				function m() {
					N ? (s(".mbsc-fr-w", T).addClass("mbsc-timer-running mbsc-timer-locked"), s(
							".mbsc-timer-btn-toggle-c > div", T).text(C.stopText), t.buttons.start.icon &&
						s(".mbsc-timer-btn-toggle-c > div", T).removeClass("mbsc-ic-" + t.buttons.start
							.icon), t.buttons.stop.icon && s(".mbsc-timer-btn-toggle-c > div", T).addClass(
							"mbsc-ic-" + t.buttons.stop.icon), "stopwatch" == C.mode && (s(
								".mbsc-timer-btn-resetlap-c > div", T).text(C.lapText), t.buttons.reset
							.icon && s(".mbsc-timer-btn-resetlap-c > div", T).removeClass("mbsc-ic-" + t
								.buttons.reset.icon), t.buttons.lap.icon && s(
								".mbsc-timer-btn-resetlap-c > div", T).addClass("mbsc-ic-" + t.buttons.lap
								.icon))) : (s(".mbsc-fr-w", T).removeClass("mbsc-timer-running"), s(
							".mbsc-timer-btn-toggle-c > div", T).text(C.startText), t.buttons.start.icon &&
						s(".mbsc-timer-btn-toggle-c > div", T).addClass("mbsc-ic-" + t.buttons.start.icon),
						t.buttons.stop.icon && s(".mbsc-timer-btn-toggle-c > div", T).removeClass(
							"mbsc-ic-" + t.buttons.stop.icon), "stopwatch" == C.mode && (s(
								".mbsc-timer-btn-resetlap-c > div", T).text(C.resetText), t.buttons.reset
							.icon && s(".mbsc-timer-btn-resetlap-c > div", T).addClass("mbsc-ic-" + t
								.buttons.reset.icon), t.buttons.lap.icon && s(
								".mbsc-timer-btn-resetlap-c > div", T).removeClass("mbsc-ic-" + t.buttons
								.lap.icon)))
				}
				var d, u, h, f, p, b, v, g, x, T, y, w = s.extend({}, t.settings),
					C = s.extend(t.settings, n, w),
					M = C.useShortLabels ? C.labelsShort : C.labels,
					S = ["toggle", "resetlap"],
					D = ["years", "months", "days", "hours", "minutes", "seconds", "fract"],
					k = {
						years: {
							index: 6,
							until: 10,
							limit: 31536e6,
							label: M[0],
							wheel: {}
						},
						months: {
							index: 5,
							until: 11,
							limit: 2592e6,
							label: M[1],
							wheel: {}
						},
						days: {
							index: 4,
							until: 31,
							limit: 864e5,
							label: M[2],
							wheel: {}
						},
						hours: {
							index: 3,
							until: 23,
							limit: 36e5,
							label: M[3],
							wheel: {}
						},
						minutes: {
							index: 2,
							until: 59,
							limit: 6e4,
							label: M[4],
							wheel: {}
						},
						seconds: {
							index: 1,
							until: 59,
							limit: 1e3,
							label: M[5],
							wheel: {}
						},
						fract: {
							index: 0,
							until: 99,
							limit: 10,
							label: M[6],
							prefix: ".",
							wheel: {}
						}
					},
					_ = {},
					A = [],
					V = 0,
					N = !1,
					F = !0,
					L = !1,
					I = Math.max(10, 1e3 * C.step),
					H = C.maxWheel,
					O = "stopwatch" == C.mode || E,
					P = C.targetTime,
					E = P && P.getTime !== e,
					Y = [
						[]
					];
				return t.start = function() {
						if (F && t.reset(), !N) {
							if (c(), !L && g >= v) return;
							N = !0, F = !1, p = new Date, f = g, C.readonly = !0, t.setVal(l(x ? g : v - g), !0,
								!0, !1, 100), u = setInterval(function() {
								g = new Date - p + f, t.setVal(l(x ? g : v - g), !0, !0, !1, Math.min(
									100, h - 10)), !L && g + h >= v && (clearInterval(u),
									setTimeout(function() {
										t.stop(), g = v, t.setVal(l(x ? g : 0), !0, !0, !1,
											100), t.trigger("onFinish", {
												time: v
											}), F = !0
									}, v - g))
							}, h), m(), t.trigger("onStart")
						}
					}, t.stop = function() {
						N && (N = !1, clearInterval(u), g = new Date - p + f, m(), t.trigger("onStop", {
							ellapsed: g
						}))
					}, t.toggle = function() {
						N ? t.stop() : t.start()
					}, t.reset = function() {
						t.stop(), g = 0, A = [], V = 0, t.setVal(l(x ? 0 : v), !0, !0, !1, 100), t.settings
							.readonly = O, F = !0, O || s(".mbsc-fr-w", T).removeClass("mbsc-timer-locked"), t
							.trigger("onReset")
					}, t.lap = function() {
						N && (b = new Date - p + f, y = b - V, V = b, A.push(b), t.trigger("onLap", {
							ellapsed: b,
							lap: y,
							laps: A
						}))
					}, t.resetlap = function() {
						N && "stopwatch" == C.mode ? t.lap() : t.reset()
					}, t.getTime = function() {
						return v
					}, t.setTime = function(e) {
						P = e / 1e3, v = e
					}, t.getElapsedTime = t.getEllapsedTime = function() {
						return N ? new Date - p + f : 0
					}, t.setElapsedTime = t.setEllapsedTime = function(e, a) {
						F || (f = g = e, p = new Date, t.setVal(l(x ? g : v - g), !0, a, !1, 100))
					}, c(!0), H || v || (H = "minutes"), "inline" !== C.display && S.push("hide"), H || s(D)
					.each(function(e, t) {
						return !H && v >= k[t].limit ? (H = t, !1) : void 0
					}), s(D).each(function(e, t) {
						r(t)
					}), h = Math.max(87, h), C.autostart && setTimeout(function() {
						t.start()
					}, 0), t.handlers.toggle = t.toggle, t.handlers.start = t.start, t.handlers.stop = t.stop, t
					.handlers.resetlap = t.resetlap, t.handlers.reset = t.reset, t.handlers.lap = t.lap, t
					.buttons.toggle = {
						parentClass: "mbsc-timer-btn-toggle-c",
						text: C.startText,
						handler: "toggle"
					}, t.buttons.start = {
						text: C.startText,
						handler: "start"
					}, t.buttons.stop = {
						text: C.stopText,
						handler: "stop"
					}, t.buttons.reset = {
						text: C.resetText,
						handler: "reset"
					}, t.buttons.lap = {
						text: C.lapText,
						handler: "lap"
					}, t.buttons.resetlap = {
						parentClass: "mbsc-timer-btn-resetlap-c",
						text: C.resetText,
						handler: "resetlap"
					}, t.buttons.hide = {
						parentClass: "mbsc-timer-btn-hide-c",
						text: C.hideText,
						handler: "cancel"
					}, {
						wheels: Y,
						headerText: !1,
						readonly: O,
						buttons: S,
						mode: "countdown",
						compClass: "mbsc-timer",
						parseValue: function() {
							return l(x ? 0 : v)
						},
						formatValue: function(e) {
							var t = "",
								a = 0;
							return s(D).each(function(s, n) {
								"fract" != n && _[n] && (t += e[a] + ("seconds" == n && _.fract ?
									"." + e[a + 1] : "") + " " + M[s] + " ", a++)
							}), t
						},
						validate: function(t) {
							var a = t.values,
								n = t.index,
								i = 0;
							F && n !== e && (P = 0, s(D).each(function(e, t) {
								_[t] && (P += k[t].limit * a[i], i++)
							}), P /= 1e3, c(!0))
						},
						onBeforeShow: function() {
							C.showLabel = !0
						},
						onMarkupReady: function(e) {
							T = s(e.target), m(), O && s(".mbsc-fr-w", T).addClass("mbsc-timer-locked")
						},
						onPosition: function(e) {
							s(".mbsc-fr-w", e.target).css("min-width", 0).css("min-width", s(
								".mbsc-fr-btn-cont", e.target)[0].offsetWidth)
						},
						onDestroy: function() {
							clearInterval(u)
						}
					}
			}
		}(),
		function(e) {
			var a = t,
				s = a.$,
				n = {
					wheelOrder: "hhiiss",
					useShortLabels: !1,
					min: 0,
					max: 1 / 0,
					labels: ["Years", "Months", "Days", "Hours", "Minutes", "Seconds"],
					labelsShort: ["Yrs", "Mths", "Days", "Hrs", "Mins", "Secs"]
				};
			a.presetShort("timespan"), a.presets.scroller.timespan = function(t) {
				function i(e) {
					var t = {};
					return s(x).each(function(a, s) {
						t[s] = C[s] ? Math.floor(e / T[s].limit) : 0, e -= t[s] * T[s].limit
					}), t
				}

				function r(e) {
					var t = !1,
						a = w[C[e] - 1] || 1,
						s = T[e],
						n = s.label,
						i = s.wheel;
					if (i.data = [], i.label = s.label, v.match(new RegExp(s.re + s.re, "i")) && (t = !0), e ==
						M) i.min = h[e], i.max = f[e], i.data = function(e) {
						return {
							value: e,
							display: o(e * a, t, n)
						}
					}, i.getIndex = function(e) {
						return Math.round(e / a)
					};
					else
						for (m = 0; m <= s.until; m += a) i.data.push({
							value: m,
							display: o(m, t, n)
						})
				}

				function o(e, t, a) {
					return (10 > e && t ? "0" : "") + e + '<span class="mbsc-ts-lbl">' + a + "</span>"
				}

				function l(e) {
					var t = 0,
						a = 0;
					return s.each(y, function(s, n) {
						isNaN(+e[t]) || (a += T[n.v].limit * e[s])
					}), a
				}

				function c(e, t) {
					return Math.floor(e / t) * t
				}
				var m, d, u, h, f, p = s.extend({}, t.settings),
					b = s.extend(t.settings, n, p),
					v = b.wheelOrder,
					g = b.useShortLabels ? b.labelsShort : b.labels,
					x = ["years", "months", "days", "hours", "minutes", "seconds"],
					T = {
						years: {
							ord: 0,
							index: 6,
							until: 10,
							limit: 31536e6,
							label: g[0],
							re: "y",
							wheel: {}
						},
						months: {
							ord: 1,
							index: 5,
							until: 11,
							limit: 2592e6,
							label: g[1],
							re: "m",
							wheel: {}
						},
						days: {
							ord: 2,
							index: 4,
							until: 31,
							limit: 864e5,
							label: g[2],
							re: "d",
							wheel: {}
						},
						hours: {
							ord: 3,
							index: 3,
							until: 23,
							limit: 36e5,
							label: g[3],
							re: "h",
							wheel: {}
						},
						minutes: {
							ord: 4,
							index: 2,
							until: 59,
							limit: 6e4,
							label: g[4],
							re: "i",
							wheel: {}
						},
						seconds: {
							ord: 5,
							index: 1,
							until: 59,
							limit: 1e3,
							label: g[5],
							re: "s",
							wheel: {}
						}
					},
					y = [],
					w = b.steps || [],
					C = {},
					M = "seconds",
					S = b.defaultValue || Math.max(b.min, Math.min(0, b.max)),
					D = [
						[]
					];
				return s(x).each(function(e, t) {
					d = v.search(new RegExp(T[t].re, "i")), d > -1 && (y.push({
						o: d,
						v: t
					}), T[t].index > T[M].index && (M = t))
				}), y.sort(function(e, t) {
					return e.o > t.o ? 1 : -1
				}), s.each(y, function(e, t) {
					C[t.v] = e + 1, D[0].push(T[t.v].wheel)
				}), h = i(b.min), f = i(b.max), s.each(y, function(e, t) {
					r(t.v)
				}), t.getVal = function(e, a) {
					return a ? t._getVal(e) : t._hasValue || e ? l(t.getArrayVal(e)) : null
				}, {
					showLabel: !0,
					wheels: D,
					compClass: "mbsc-ts",
					parseValue: function(e) {
						var t, n = [];
						return a.util.isNumeric(e) || !e ? (u = i(e || S), s.each(y, function(e, t) {
							n.push(u[t.v])
						})) : s.each(y, function(a, s) {
							t = new RegExp("(\\d+)\\s?(" + b.labels[T[s.v].ord] + "|" + b
								.labelsShort[T[s.v].ord] + ")", "gi").exec(e), n.push(t ? t[
								1] : 0)
						}), s(n).each(function(e, t) {
							n[e] = c(t, w[e] || 1)
						}), n
					},
					formatValue: function(e) {
						var t = "";
						return s.each(y, function(a, s) {
							t += +e[a] ? e[a] + " " + T[s.v].label + " " : ""
						}), t ? t.replace(/\s+$/g, "") : 0
					},
					validate: function(a) {
						var n, r, o, c, m = a.values,
							d = a.direction,
							u = [],
							p = !0,
							b = !0;
						return s(x).each(function(a, v) {
							if (C[v] !== e) {
								if (o = C[v] - 1, u[o] = [], c = {}, v != M) {
									if (p)
										for (r = f[v] + 1; r <= T[v].until; r++) c[r] = !0;
									if (b)
										for (r = 0; r < h[v]; r++) c[r] = !0
								}
								m[o] = t.getValidValue(o, m[o], d, c), n = i(l(m)), p = p && n[
									v] == f[v], b = b && n[v] == h[v], s.each(c, function(
								e) {
									u[o].push(e)
								})
							}
						}), {
							disabled: u
						}
					}
				}
			}
		}(),
		function() {
			var e = t,
				a = e.$,
				s = e.classes;
			s.Widget = function(e, n, i) {
				function r(e) {
					a(".mbsc-fr-c", e), !a(".mbsc-fr-c", e).hasClass("mbsc-wdg-c") && t.vKMaI && (a(
							".mbsc-fr-c", e).addClass("mbsc-wdg-c").append(m.show()), a(".mbsc-w-p", e)
						.length || a(".mbsc-fr-c", e).addClass("mbsc-w-p"))
				}
				var o, l, c, m = a(e),
					d = this;
				s.Frame.call(this, e, n, !0), d._generateContent = function() {
					return ""
				}, d._markupReady = function(e) {
					"inline" != o.display && r(e)
				}, d._markupInserted = function(e) {
					"inline" == o.display && r(e), e.trigger("mbsc-enhance", [{
						theme: o.theme,
						lang: o.lang
					}])
				}, d._markupRemove = function() {
					m.hide(), l ? l.prepend(m) : c.after(m)
				}, d._processSettings = function() {
					o = d.settings, d.buttons.close = {
						text: o.closeText,
						handler: "cancel"
					}, d.buttons.ok = {
						text: o.okText,
						handler: "set"
					}, o.buttons = o.buttons || ("inline" == o.display ? [] : ["ok"]), o.cssClass = (o
						.cssClass || "") + " mbsc-wdg", l || c || (c = m.prev(), c.length || (l = m
						.parent())), m.hide()
				}, i || d.init(n)
			}, s.Widget.prototype = {
				_hasDef: !0,
				_hasTheme: !0,
				_hasContent: !0,
				_class: "widget",
				_defaults: a.extend({}, s.Frame.prototype._defaults, {
					okText: "OK"
				})
			}, e.themes.widget = e.themes.frame, e.presetShort("widget", "Widget", !1)
		}(),
		function() {
			function e(e, t) {
				var s = c(t, "X", !0),
					i = c(t, "Y", !0),
					o = e.offset(),
					l = s - o.left,
					m = i - o.top,
					d = Math.max(l, e[0].offsetWidth - l),
					u = Math.max(m, e[0].offsetHeight - m),
					h = 2 * Math.sqrt(Math.pow(d, 2) + Math.pow(u, 2));
				a(n), n = r('<span class="mbsc-ripple"></span>').css({
					width: h,
					height: h,
					top: i - o.top - h / 2,
					left: s - o.left - h / 2
				}).appendTo(e), setTimeout(function() {
					n.addClass("mbsc-ripple-scaled mbsc-ripple-visible")
				}, 10)
			}

			function a(e) {
				setTimeout(function() {
					e && (e.removeClass("mbsc-ripple-visible"), setTimeout(function() {
						e.remove()
					}, 2e3))
				}, 100)
			}
			var s, n, i = t,
				r = i.$,
				o = i.util,
				l = o.testTouch,
				c = o.getCoord;
			i.themes.material = {
				addRipple: e,
				removeRipple: function() {
					a(n)
				},
				initRipple: function(t, i, o, m) {
					var d, u;
					t.off(".mbsc-ripple").on("touchstart.mbsc-ripple mousedown.mbsc-ripple", i, function(
					t) {
						l(t, this) && (d = c(t, "X"), u = c(t, "Y"), s = r(this), s.hasClass(o) || s
							.hasClass(m) ? s = null : e(s, t))
					}).on("touchmove.mbsc-ripple mousemove.mbsc-ripple", i, function(e) {
						(s && Math.abs(c(e, "X") - d) > 9 || Math.abs(c(e, "Y") - u) > 9) && (a(n),
							s = null)
					}).on(
						"touchend.mbsc-ripple touchcancel.mbsc-ripple mouseleave.mbsc-ripple mouseup.mbsc-ripple",
						i,
						function() {
							s && (setTimeout(function() {
								a(n)
							}, 100), s = null)
						})
				}
			}
		}(),
		function() {
			t.themes.frame["ios-dark"] = {
				baseTheme: "ios",
				display: "bottom",
				dateOrder: "MMdyy",
				rows: 5,
				height: 34,
				minWidth: 55,
				scroll3d: !0,
				headerText: !1,
				showLabel: !1,
				btnWidth: !1,
				selectedLineBorder: 1,
				useShortLabels: !0,
				deleteIcon: "ios-backspace",
				checkIcon: "ion-ios7-checkmark-empty",
				btnCalPrevClass: "mbsc-ic mbsc-ic-arrow-left5",
				btnCalNextClass: "mbsc-ic mbsc-ic-arrow-right5",
				btnPlusClass: "mbsc-ic mbsc-ic-arrow-down5",
				btnMinusClass: "mbsc-ic mbsc-ic-arrow-up5"
			}, t.themes.listview["ios-dark"] = {
				baseTheme: "ios"
			}, t.themes.menustrip["ios-dark"] = {
				baseTheme: "ios"
			}, t.themes.form["ios-dark"] = {
				baseTheme: "ios"
			}, t.themes.progress["ios-dark"] = {
				baseTheme: "ios"
			}
		}(),
		function() {
			var e = t.$;
			t.themes.frame["material-dark"] = {
				baseTheme: "material",
				showLabel: !1,
				headerText: !1,
				btnWidth: !1,
				selectedLineBorder: 2,
				dateOrder: "MMddyy",
				weekDays: "min",
				deleteIcon: "material-backspace",
				icon: {
					filled: "material-star",
					empty: "material-star-outline"
				},
				checkIcon: "material-check",
				btnPlusClass: "mbsc-ic mbsc-ic-material-keyboard-arrow-down",
				btnMinusClass: "mbsc-ic mbsc-ic-material-keyboard-arrow-up",
				btnCalPrevClass: "mbsc-ic mbsc-ic-material-keyboard-arrow-left",
				btnCalNextClass: "mbsc-ic mbsc-ic-material-keyboard-arrow-right",
				onMarkupReady: function(a) {
					t.themes.material.initRipple(e(a.target), ".mbsc-fr-btn-e", "mbsc-fr-btn-d",
						"mbsc-fr-btn-nhl")
				},
				onEventBubbleShow: function(t) {
					var a = e(t.eventList),
						s = e(t.target).closest(".mbsc-cal-row").index() < 2,
						n = e(".mbsc-cal-event-color", a).eq(s ? 0 : -1).css("background-color");
					e(".mbsc-cal-events-arr", a).css("border-color", s ? "transparent transparent " + n +
						" transparent" : n + "transparent transparent transparent")
				}
			}, t.themes.listview["material-dark"] = {
				baseTheme: "material",
				onItemActivate: function(a) {
					t.themes.material.addRipple(e(a.target), a.domEvent)
				},
				onItemDeactivate: function() {
					t.themes.material.removeRipple()
				},
				onSlideStart: function(t) {
					e(".mbsc-ripple", t.target).remove()
				},
				onSortStart: function(t) {
					e(".mbsc-ripple", t.target).remove()
				}
			}, t.themes.menustrip["material-dark"] = {
				baseTheme: "material",
				onInit: function() {
					t.themes.material.initRipple(e(this), ".mbsc-ms-item", "mbsc-btn-d", "mbsc-btn-nhl")
				}
			}, t.themes.form["material-dark"] = {
				baseTheme: "material",
				onControlActivate: function(a) {
					var s, n = e(a.target);
					("button" == n[0].type || "submit" == n[0].type) && (s = n), "segmented" == n.attr(
							"data-role") && (s = n.next()), n.hasClass("mbsc-stepper-control") && !n
						.hasClass("mbsc-step-disabled") && (s = n.find(".mbsc-segmented-content")), s && t
						.themes.material.addRipple(s, a.domEvent)
				},
				onControlDeactivate: function() {
					t.themes.material.removeRipple()
				}
			}, t.themes.progress["material-dark"] = {
				baseTheme: "material"
			}
		}(),
		function() {
			t.themes.frame["android-holo-light"] = {
				baseTheme: "android-holo",
				dateOrder: "Mddyy",
				rows: 5,
				minWidth: 76,
				height: 36,
				showLabel: !1,
				selectedLineBorder: 2,
				useShortLabels: !0,
				icon: {
					filled: "star3",
					empty: "star"
				},
				btnPlusClass: "mbsc-ic mbsc-ic-arrow-down6",
				btnMinusClass: "mbsc-ic mbsc-ic-arrow-up6"
			}, t.themes.listview["android-holo-light"] = {
				baseTheme: "android-holo"
			}, t.themes.menustrip["android-holo-light"] = {
				baseTheme: "android-holo"
			}, t.themes.form["android-holo-light"] = {
				baseTheme: "android-holo"
			}, t.themes.progress["android-holo-light"] = {
				baseTheme: "android-holo"
			}
		}(),
		function() {
			var e = t.$;
			t.themes.frame["wp-light"] = {
				baseTheme: "wp",
				minWidth: 76,
				height: 76,
				dateDisplay: "mmMMddDDyy",
				headerText: !1,
				showLabel: !1,
				deleteIcon: "backspace4",
				icon: {
					filled: "star3",
					empty: "star"
				},
				btnWidth: !1,
				btnCalPrevClass: "mbsc-ic mbsc-ic-arrow-left2",
				btnCalNextClass: "mbsc-ic mbsc-ic-arrow-right2",
				btnPlusClass: "mbsc-ic mbsc-ic-plus",
				btnMinusClass: "mbsc-ic mbsc-ic-minus",
				onMarkupInserted: function(t, a) {
					function s(t) {
						return e.isArray(l.readonly) ? l.readonly[t] : l.readonly
					}
					var n, i, r, o = t.target,
						l = a.settings;
					e(".mbsc-sc-whl", o).on("touchstart mousedown wheel mousewheel", function(t) {
						"mousedown" === t.type && i || s(e(this).attr("data-index")) || (i =
							"touchstart" === t.type, n = !0, r = e(this).hasClass(
								"mbsc-sc-whl-wpa"), e(".mbsc-sc-whl", o).removeClass(
								"mbsc-sc-whl-wpa"), e(this).addClass("mbsc-sc-whl-wpa"))
					}).on("touchmove mousemove", function() {
						n = !1
					}).on("touchend mouseup", function(t) {
						n && r && e(t.target).closest(".mbsc-sc-itm").hasClass("mbsc-sc-itm-sel") &&
							e(this).removeClass("mbsc-sc-whl-wpa"), "mouseup" === t.type && (i = !
							1), n = !1
					})
				},
				onInit: function(e, t) {
					var a = t.buttons;
					a.set.icon = "checkmark", a.cancel.icon = "close", a.clear.icon = "close", a.ok && (a.ok
						.icon = "checkmark"), a.close && (a.close.icon = "close"), a.now && (a.now
						.icon = "loop2"), a.toggle && (a.toggle.icon = "play3"), a.start && (a.start
						.icon = "play3"), a.stop && (a.stop.icon = "pause2"), a.reset && (a.reset.icon =
						"stop2"), a.lap && (a.lap.icon = "loop2"), a.hide && (a.hide.icon = "close")
				}
			}, t.themes.listview["wp-light"] = {
				baseTheme: "wp"
			}, t.themes.menustrip["wp-light"] = {
				baseTheme: "wp"
			}, t.themes.form["wp-light"] = {
				baseTheme: "wp"
			}, t.themes.progress["wp-light"] = {
				baseTheme: "wp"
			}
		}(),
		function() {
			t.themes.frame["mobiscroll-dark"] = {
				baseTheme: "mobiscroll",
				rows: 5,
				showLabel: !1,
				headerText: !1,
				btnWidth: !1,
				selectedLineBorder: 1,
				dateOrder: "MMddyy",
				weekDays: "min",
				checkIcon: "ion-ios7-checkmark-empty",
				btnPlusClass: "mbsc-ic mbsc-ic-arrow-down5",
				btnMinusClass: "mbsc-ic mbsc-ic-arrow-up5",
				btnCalPrevClass: "mbsc-ic mbsc-ic-arrow-left5",
				btnCalNextClass: "mbsc-ic mbsc-ic-arrow-right5"
			}, t.themes.listview["mobiscroll-dark"] = {
				baseTheme: "mobiscroll"
			}, t.themes.menustrip["mobiscroll-dark"] = {
				baseTheme: "mobiscroll"
			}, t.themes.form["mobiscroll-dark"] = {
				baseTheme: "mobiscroll"
			}, t.themes.progress["mobiscroll-dark"] = {
				baseTheme: "mobiscroll"
			}
		}(),
		function() {
			var e, a, s = t,
				n = s.platform,
				i = s.themes,
				r = s.$;
			"android" == n.name ? e = n.majorVersion >= 5 ? "material" : "android-holo" : "ios" == n.name ? e =
				"ios" : "wp" == n.name && (e = "wp"), r.each(i, function(t, n) {
					return r.each(n, function(t, n) {
						return n.baseTheme == e && "android-holo-light" != t && "material-dark" != t &&
							"wp-light" != t && "ios-dark" != t ? (s.autoTheme = t, a = !0, !1) : void(
								t == e && (s.autoTheme = t))
					}), a ? !1 : void 0
				})
		}(), t
});
