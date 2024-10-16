// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../chunks/_rollupPluginBabelHelpers ../../core/has ../../core/maybe ./checkWebGLError ./enums ./ShaderTranspiler".split(" "),function(p,u,q,n,r,m,v){function h(g,f){if(n.isNone(g)||g.length!==f.length)return!0;for(let b=0;b<g.length;++b)if(g[b]!==f[b])return!0;return!1}function t(g,f,b){const a="webgl2"===g.webglVersion?v.transpileShader(b,f):b,c=g.gl,d=c.createShader(f);c.shaderSource(d,a);c.compileShader(d);r.webglValidateShadersEnabled()&&!c.getShaderParameter(d,c.COMPILE_STATUS)&&
(console.error("Compile error in ".concat(35633===f?"vertex":"fragment"," shader")),console.error(c.getShaderInfoLog(d)),console.error(w(a)),"webgl2"===g.webglVersion&&(console.log("Shader source before transpilation:"),console.log(b)));return d}function w(g){let f=2;return g.replace(/\n/g,()=>{var b=f++;b=1E3<=b?b.toString():("  "+b).slice(-3);return"\n"+b+":"})}function k(g,f){for(let b=0;b<g.length;++b)f[b]=g[b]}q=function(){function g(b,a,c,d){this._context=b;this._locations=d;this._nameToUniformLocation=
{};this._nameToUniform1={};this._nameToUniform1v={};this._nameToUniform2={};this._nameToUniform3={};this._nameToUniform4={};this._nameToUniformMatrix3={};this._nameToUniformMatrix4={};b||console.error("RenderingContext isn't initialized!");0===a.length&&console.error("Shaders source should not be empty!");this._vShader=t(this._context,35633,a);this._fShader=t(this._context,35632,c);this._vShader&&this._fShader||console.error("Error loading shaders!");this._context.instanceCounter.increment(m.ResourceType.VertexShader,
this);this._context.instanceCounter.increment(m.ResourceType.FragmentShader,this)}var f=g.prototype;f.dispose=function(){const b=this._context.gl;this._vShader&&(b.deleteShader(this._vShader),this._vShader=null,this._context.instanceCounter.decrement(m.ResourceType.VertexShader,this));this._fShader&&(b.deleteShader(this._fShader),this._fShader=null,this._context.instanceCounter.decrement(m.ResourceType.FragmentShader,this));this._glName&&(b.deleteProgram(this._glName),this._glName=null,this._context.instanceCounter.decrement(m.ResourceType.Program,
this))};f._getUniformLocation=function(b){void 0===this._nameToUniformLocation[b]&&(this._nameToUniformLocation[b]=this._context.gl.getUniformLocation(this.glName,b));return this._nameToUniformLocation[b]};f.hasUniform=function(b){return null!==this._getUniformLocation(b)};f.setUniform1i=function(b,a){const c=this._nameToUniform1[b];if(void 0===c||a!==c)this._context.useProgram(this),this._context.gl.uniform1i(this._getUniformLocation(b),a),this._nameToUniform1[b]=a};f.setUniform1iv=function(b,a){const c=
this._nameToUniform1v[b];h(c,a)&&(this._context.useProgram(this),this._context.gl.uniform1iv(this._getUniformLocation(b),a),void 0===c?this._nameToUniform1v[b]=Array.from(a):k(a,c))};f.setUniform2iv=function(b,a){const c=this._nameToUniform2[b];h(c,a)&&(this._context.useProgram(this),this._context.gl.uniform2iv(this._getUniformLocation(b),a),void 0===c?this._nameToUniform2[b]=Array.from(a):k(a,c))};f.setUniform3iv=function(b,a){const c=this._nameToUniform3[b];h(c,a)&&(this._context.useProgram(this),
this._context.gl.uniform3iv(this._getUniformLocation(b),a),void 0===c?this._nameToUniform3[b]=Array.from(a):k(a,c))};f.setUniform4iv=function(b,a){const c=this._nameToUniform4[b];h(c,a)&&(this._context.useProgram(this),this._context.gl.uniform4iv(this._getUniformLocation(b),a),void 0===c?this._nameToUniform4[b]=Array.from(a):k(a,c))};f.setUniform1f=function(b,a){const c=this._nameToUniform1[b];if(void 0===c||a!==c)this._context.useProgram(this),this._context.gl.uniform1f(this._getUniformLocation(b),
a),this._nameToUniform1[b]=a};f.setUniform1fv=function(b,a){const c=this._nameToUniform1v[b];h(c,a)&&(this._context.useProgram(this),this._context.gl.uniform1fv(this._getUniformLocation(b),a),void 0===c?this._nameToUniform1v[b]=Array.from(a):k(a,c))};f.setUniform2f=function(b,a,c){const d=this._nameToUniform2[b];if(void 0===d||a!==d[0]||c!==d[1])this._context.useProgram(this),this._context.gl.uniform2f(this._getUniformLocation(b),a,c),void 0===d?this._nameToUniform2[b]=[a,c]:(d[0]=a,d[1]=c)};f.setUniform2fv=
function(b,a){const c=this._nameToUniform2[b];h(c,a)&&(this._context.useProgram(this),this._context.gl.uniform2fv(this._getUniformLocation(b),a),void 0===c?this._nameToUniform2[b]=Array.from(a):k(a,c))};f.setUniform3f=function(b,a,c,d){const e=this._nameToUniform3[b];if(void 0===e||a!==e[0]||c!==e[1]||d!==e[2])this._context.useProgram(this),this._context.gl.uniform3f(this._getUniformLocation(b),a,c,d),void 0===e?this._nameToUniform3[b]=[a,c,d]:(e[0]=a,e[1]=c,e[2]=d)};f.setUniform3fv=function(b,a){const c=
this._nameToUniform3[b];h(c,a)&&(this._context.useProgram(this),this._context.gl.uniform3fv(this._getUniformLocation(b),a),void 0===c?this._nameToUniform3[b]=Array.from(a):k(a,c))};f.setUniform4f=function(b,a,c,d,e){const l=this._nameToUniform4[b];if(void 0===l||a!==l[0]||c!==l[1]||d!==l[2]||e!==l[3])this._context.useProgram(this),this._context.gl.uniform4f(this._getUniformLocation(b),a,c,d,e),void 0===l?this._nameToUniform4[b]=[a,c,d,e]:(l[0]=a,l[1]=c,l[2]=d,l[3]=e)};f.setUniform4fv=function(b,a){const c=
this._nameToUniform4[b];h(c,a)&&(this._context.useProgram(this),this._context.gl.uniform4fv(this._getUniformLocation(b),a),void 0===c?this._nameToUniform4[b]=Array.from(a):k(a,c))};f.setUniformMatrix3fv=function(b,a,c=!1,d=!1){const e=this._nameToUniformMatrix3[b];d=d?e!==a:n.isNone(e)?!0:9!==e.length?h(e,a):9!==e.length||e[0]!==a[0]||e[1]!==a[1]||e[2]!==a[2]||e[3]!==a[3]||e[4]!==a[4]||e[5]!==a[5]||e[6]!==a[6]||e[7]!==a[7]||e[8]!==a[8];d&&(this._context.useProgram(this),this._context.gl.uniformMatrix3fv(this._getUniformLocation(b),
c,a),void 0===e?this._nameToUniformMatrix3[b]=Array.from(a):k(a,e))};f.setUniformMatrix4fv=function(b,a,c=!1){const d=this._nameToUniformMatrix4[b];var e=n.isNone(d)?!0:16!==d.length?h(d,a):16!==d.length||d[0]!==a[0]||d[1]!==a[1]||d[2]!==a[2]||d[3]!==a[3]||d[4]!==a[4]||d[5]!==a[5]||d[6]!==a[6]||d[7]!==a[7]||d[8]!==a[8]||d[9]!==a[9]||d[10]!==a[10]||d[11]!==a[11]||d[12]!==a[12]||d[13]!==a[13]||d[14]!==a[14]||d[15]!==a[15];e&&(this._context.useProgram(this),this._context.gl.uniformMatrix4fv(this._getUniformLocation(b),
c,a),void 0===d?this._nameToUniformMatrix4[b]=Array.from(a):k(a,d))};f.assertCompatibleVertexAttributeLocations=function(b){b.locations!==this._locations&&console.error("VertexAttributeLocations are incompatible")};f.stop=function(){};u._createClass(g,[{key:"glName",get:function(){if(n.isSome(this._glName))return this._glName;if(n.isNone(this._vShader))return null;const b=this._context.gl,a=b.createProgram();b.attachShader(a,this._vShader);b.attachShader(a,this._fShader);this._locations.forEach((c,
d)=>b.bindAttribLocation(a,c,d));b.linkProgram(a);r.webglValidateShadersEnabled()&&!b.getProgramParameter(a,b.LINK_STATUS)&&console.error("Could not link shader\n"+`validated: ${b.getProgramParameter(a,b.VALIDATE_STATUS)}`+`, gl error ${b.getError()}`+`, vertex: ${b.getShaderParameter(this._vShader,b.COMPILE_STATUS)}`+`, fragment: ${b.getShaderParameter(this._fShader,b.COMPILE_STATUS)}`+`, info log: ${b.getProgramInfoLog(a)}`);this._glName=a;this._context.instanceCounter.increment(m.ResourceType.Program,
this);return a}}]);return g}();p.Program=q;Object.defineProperty(p,"__esModule",{value:!0})});