"use strict";(self.webpackChunkRemoteClient=self.webpackChunkRemoteClient||[]).push([[9314],{79314:(e,t,s)=>{s.r(t),s.d(t,{toBinaryGLTF:()=>U});var i,r,n,a,o,h,c,f,u=s(50406),l=s(91597),d=s(76506),p=s(91306),g=s(35861),m=s(45087),w=s(82426),b=s(71337),y=s(84017),A=(s(74569),s(60991)),x=s(32101);s(92143),s(31450),s(71552),s(40642),s(34250),s(86656),s(22723),s(86787),s(97714),s(17533),s(2906),s(73796),s(74673),s(21972),s(23639),s(91055),s(6906),s(60947),s(35132),s(89623),s(29794),s(48027),s(54174),s(44265),s(68225),s(38742),s(47029),s(53785),s(57251),s(95587),s(65775),s(26923),s(1623),s(21801),s(84069),s(12381),s(73173),s(82058),s(88762),s(44567),s(98380),s(92896),s(92482),s(92624),s(92847),s(34987),s(40167),s(72836),s(84661),s(22781),s(32422);class E{constructor(e,t){if(!e)throw new Error("GLB requires a JSON gltf chunk");this.length=E.HEADER_SIZE,this.length+=E.CHUNK_HEADER_SIZE;const s=this.textToArrayBuffer(e);if(this.length+=this.alignTo(s.byteLength,4),t&&(this.length+=E.CHUNK_HEADER_SIZE,this.length+=t.byteLength,t.byteLength%4))throw new Error("Expected BIN chunk length to be divisible by 4 at this point");this.buffer=new ArrayBuffer(this.length),this.outView=new DataView(this.buffer),this.writeHeader();const i=this.writeChunk(s,12,1313821514,32);t&&this.writeChunk(t,i,5130562)}writeHeader(){this.outView.setUint32(0,E.MAGIC,!0),this.outView.setUint32(4,E.VERSION,!0),this.outView.setUint32(8,this.length,!0)}writeChunk(e,t,s,i=0){const r=this.alignTo(e.byteLength,4);for(this.outView.setUint32(t,r,!0),this.outView.setUint32(t+=4,s,!0),this.writeArrayBuffer(this.outView.buffer,e,t+=4,0,e.byteLength),t+=e.byteLength;t%4;)i&&this.outView.setUint8(t,i),t++;return t}writeArrayBuffer(e,t,s,i,r){new Uint8Array(e,s,r).set(new Uint8Array(t,i,r),0)}textToArrayBuffer(e){if((0,d.h)("esri-text-encoder"))return(new TextEncoder).encode(e).buffer;const t=new Uint8Array(e.length);for(let s=0;s<t.length;++s)t[s]=e.charCodeAt(s);return t.buffer}alignTo(e,t){return t*Math.ceil(e/t)}}E.HEADER_SIZE=12,E.CHUNK_HEADER_SIZE=8,E.MAGIC=1179937895,E.VERSION=2,(f=i||(i={}))[f.External=0]="External",f[f.DataURI=1]="DataURI",f[f.GLB=2]="GLB",function(e){e[e.External=0]="External",e[e.DataURI=1]="DataURI",e[e.GLB=2]="GLB"}(r||(r={})),function(e){e[e.ARRAY_BUFFER=34962]="ARRAY_BUFFER",e[e.ELEMENT_ARRAY_BUFFER=34963]="ELEMENT_ARRAY_BUFFER"}(n||(n={})),function(e){e.SCALAR="SCALAR",e.VEC2="VEC2",e.VEC3="VEC3",e.VEC4="VEC4",e.MAT2="MAT2",e.MAT3="MAT3",e.MAT4="MAT4"}(a||(a={})),function(e){e[e.POINTS=0]="POINTS",e[e.LINES=1]="LINES",e[e.LINE_LOOP=2]="LINE_LOOP",e[e.LINE_STRIP=3]="LINE_STRIP",e[e.TRIANGLES=4]="TRIANGLES",e[e.TRIANGLE_STRIP=5]="TRIANGLE_STRIP",e[e.TRIANGLE_FAN=6]="TRIANGLE_FAN"}(o||(o={})),function(e){e.OPAQUE="OPAQUE",e.MASK="MASK",e.BLEND="BLEND"}(h||(h={})),function(e){e[e.NoColor=0]="NoColor",e[e.FaceColor=1]="FaceColor",e[e.VertexColor=2]="VertexColor"}(c||(c={}));class T{constructor(e,t,s,i,r){this.buffer=e,this.componentType=s,this.dataType=i,this.data=[],this.isFinalized=!1,this.accessorIndex=-1,this.accessorAttribute=null,this.accessorMin=null,this.accessorMax=null,t.bufferViews||(t.bufferViews=[]),this.index=t.bufferViews.length,this.bufferView={buffer:e.index,byteLength:-1,target:r};const a=this.getElementSize();a>=4&&r!==n.ELEMENT_ARRAY_BUFFER&&(this.bufferView.byteStride=a),t.bufferViews.push(this.bufferView),this.numComponentsForDataType=this.calculateNumComponentsForDataType()}push(e){const t=this.data.length;if(this.data.push(e),this.accessorIndex>=0){const s=t%this.numComponentsForDataType,i=this.accessorMin[s];this.accessorMin[s]="number"!=typeof i?e:Math.min(i,e);const r=this.accessorMax[s];this.accessorMax[s]="number"!=typeof r?e:Math.max(r,e)}}get dataSize(){return this.data.length*this.sizeComponentType()}get byteSize(){return e=this.dataSize,4*Math.ceil(e/4);var e}getByteOffset(){if(!this.isFinalized)throw new Error("Cannot get BufferView offset until it is finalized");return this.buffer.getByteOffset(this)}get byteOffset(){if(!this.isFinalized)throw new Error("Cannot get BufferView offset until it is finalized");return this.buffer.getByteOffset(this)}createTypedArray(e,t){switch(this.componentType){case 5120:return new Int8Array(e,t);case 5126:return new Float32Array(e,t);case 5122:return new Int16Array(e,t);case 5121:return new Uint8Array(e,t);case 5125:return new Uint32Array(e,t);case 5123:return new Uint16Array(e,t)}}writeOutToBuffer(e,t){this.createTypedArray(e,t).set(this.data)}writeAsync(e){if(this.asyncWritePromise)throw new Error("Can't write multiple bufferView values asynchronously");return this.asyncWritePromise=e.then((e=>{const t=new Uint8Array(e);for(let e=0;e<t.length;++e)this.data.push(t[e]);delete this.asyncWritePromise})),this.asyncWritePromise}startAccessor(e){if(this.accessorIndex>=0)throw new Error("Accessor was started without ending the previous one");this.accessorIndex=this.data.length,this.accessorAttribute=e;const t=this.numComponentsForDataType;this.accessorMin=new Array(t),this.accessorMax=new Array(t)}endAccessor(){if(this.accessorIndex<0)throw new Error("An accessor was not started, but was attempted to be ended");const e=this.getElementSize(),t=this.numComponentsForDataType,s=(this.data.length-this.accessorIndex)/t;if(s%1)throw new Error("An accessor was ended with missing component values");for(let e=0;e<this.accessorMin.length;++e)"number"!=typeof this.accessorMin[e]&&(this.accessorMin[e]=0),"number"!=typeof this.accessorMax[e]&&(this.accessorMax[e]=0);const i={byteOffset:e*(this.accessorIndex/t),componentType:this.componentType,count:s,type:this.dataType,min:this.accessorMin,max:this.accessorMax,name:this.accessorAttribute};switch(this.accessorAttribute){case"TEXCOORD_0":case"TEXCOORD_1":case"COLOR_0":case"WEIGHTS_0":switch(this.componentType){case 5121:case 5123:i.normalized=!0}}return this.accessorIndex=-1,this.accessorAttribute=null,this.accessorMin=null,this.accessorMax=null,i}get finalized(){return this.finalizedPromise?this.finalizedPromise:this.isFinalized?this.finalizedPromise=Promise.resolve():this.finalizedPromise=new Promise((e=>this.finalizedPromiseResolve=e))}finalize(){const e=this.bufferView;return new Promise((e=>{const t=this.buffer.getViewFinalizePromises(this);this.asyncWritePromise&&t.push(this.asyncWritePromise),e((0,u.as)(t))})).then((()=>{this.isFinalized=!0,e.byteOffset=this.getByteOffset(),e.byteLength=this.dataSize,this.finalizedPromiseResolve&&this.finalizedPromiseResolve()}))}getElementSize(){return this.sizeComponentType()*this.numComponentsForDataType}sizeComponentType(){switch(this.componentType){case 5120:case 5121:return 1;case 5122:case 5123:return 2;case 5125:case 5126:return 4}}calculateNumComponentsForDataType(){switch(this.dataType){case a.SCALAR:return 1;case a.VEC2:return 2;case a.VEC3:return 3;case a.VEC4:case a.MAT2:return 4;case a.MAT3:return 9;case a.MAT4:return 16}}}class R{constructor(e){this.gltf=e,this.bufferViews=[],this.isFinalized=!1,e.buffers||(e.buffers=[]),this.index=e.buffers.length;const t={byteLength:-1};e.buffers.push(t),this.buffer=t}addBufferView(e,t,s){if(this.finalizePromise)throw new Error("Cannot add buffer view after fiinalizing buffer");const i=new T(this,this.gltf,e,t,s);return this.bufferViews.push(i),i}getByteOffset(e){let t=0;for(const s of this.bufferViews){if(s===e)return t;t+=s.byteSize}throw new Error("Given bufferView was not present in this buffer")}getViewFinalizePromises(e){const t=[];for(const s of this.bufferViews){if(e&&s===e)return t;t.push(s.finalized)}return t}getArrayBuffer(){if(!this.isFinalized)throw new Error("Cannot get ArrayBuffer from Buffer before it is finalized");const e=this.getTotalSize(),t=new ArrayBuffer(e);let s=0;for(const e of this.bufferViews)e.writeOutToBuffer(t,s),s+=e.byteSize;return t}finalize(){if(this.finalizePromise)throw new Error(`Buffer ${this.index} was already finalized`);return this.finalizePromise=new Promise((e=>{e((0,u.as)(this.getViewFinalizePromises()))})).then((()=>{this.isFinalized=!0;const e=this.getArrayBuffer();this.buffer.byteLength=e.byteLength,this.buffer.uri=e})),this.gltf.extras.promises.push(this.finalizePromise),this.finalizePromise}getTotalSize(){let e=0;for(const t of this.bufferViews)e+=t.byteSize;return e}}function C(e,t){(0,d.b)(t.normal)&&(t.normal=new Float32Array(t.position.length));const s=e.faces,{position:i,normal:r}=t,n=s.length/3;for(let e=0;e<n;++e){const t=3*s[3*e+0],n=3*s[3*e+1],a=3*s[3*e+2],o=(0,w.e)(B,i[t+0],i[t+1],i[t+2]),h=(0,w.e)(O,i[n+0],i[n+1],i[n+2]),c=(0,w.e)(M,i[a+0],i[a+1],i[a+2]),f=(0,w.s)(h,h,o),u=(0,w.s)(c,c,o),l=(0,w.a)(f,f,u);r[t+0]+=l[0],r[t+1]+=l[1],r[t+2]+=l[2],r[n+0]+=l[0],r[n+1]+=l[1],r[n+2]+=l[2],r[a+0]+=l[0],r[a+1]+=l[1],r[a+2]+=l[2]}for(let e=0;e<r.length;e+=3)(0,w.e)(L,r[e],r[e+1],r[e+2]),(0,w.n)(L,L),r[e+0]=L[0],r[e+1]=L[1],r[e+2]=L[2]}const B=(0,w.b)(),O=(0,w.b)(),M=(0,w.b)(),L=(0,w.b)();async function S(e){const t=I(e);if((0,d.b)(t))throw new A.Z("imageToArrayBuffer","Unsupported image type");const s=await async function(e){if(!(e instanceof HTMLImageElement))return"image/png";const t=e.src;if((0,x.HK)(t)){const{mediaType:e}=(0,x.sJ)(t);return"image/jpeg"===e?e:"image/png"}return/\.png$/i.test(t)?"image/png":/\.(jpg|jpeg)$/i.test(t)?"image/jpeg":"image/png"}(e),i=await new Promise((e=>t.toBlob(e,s)));if(!i)throw new A.Z("imageToArrayBuffer","Failed to encode image");return{data:await i.arrayBuffer(),type:s}}function I(e){if(e instanceof HTMLCanvasElement)return e;if(e instanceof HTMLVideoElement)return null;const t=document.createElement("canvas");t.width=e.width,t.height=e.height;const s=t.getContext("2d");return e instanceof HTMLImageElement?s.drawImage(e,0,0,e.width,e.height):e instanceof ImageData&&s.putImageData(e,e.width,e.height),t}class z{constructor(e,t,s){this.params={},this.materialMap=new Array,this.imageMap=new Map,this.textureMap=new Map,this.gltf={asset:{version:"2.0",copyright:e.copyright,generator:e.generator},extras:{options:t,binChunkBuffer:null,promises:[]}},s&&(this.params=s),this.addScenes(e)}addScenes(e){this.gltf.scene=e.defaultScene;const t=this.gltf.extras.options.bufferOutputType===i.GLB||this.gltf.extras.options.imageOutputType===r.GLB;t&&(this.gltf.extras.binChunkBuffer=new R(this.gltf)),e.forEachScene((e=>{this.addScene(e)})),t&&this.gltf.extras.binChunkBuffer.finalize()}addScene(e){this.gltf.scenes||(this.gltf.scenes=[]);const t={};e.name&&(t.name=e.name),e.forEachNode((e=>{t.nodes||(t.nodes=[]);const s=this.addNode(e);t.nodes.push(s)})),this.gltf.scenes.push(t)}addNode(e){this.gltf.nodes||(this.gltf.nodes=[]);const t={};e.name&&(t.name=e.name);const s=e.translation;(0,w.p)(s,w.Z)||(t.translation=(0,w.q)(s));const i=e.rotation;(0,g.e)(i,m.I)||(t.rotation=(0,m.e)(i));const r=e.scale;(0,w.p)(r,w.O)||(t.scale=(0,w.q)(r)),e.mesh&&e.mesh.vertexAttributes.position?t.mesh=this.addMesh(e.mesh):e.forEachNode((e=>{t.children||(t.children=[]);const s=this.addNode(e);t.children.push(s)}));const n=this.gltf.nodes.length;return this.gltf.nodes.push(t),n}addMesh(e){this.gltf.meshes||(this.gltf.meshes=[]);const t={primitives:[]},s=this.gltf.extras.options.bufferOutputType===i.GLB;let r;r=s?this.gltf.extras.binChunkBuffer:new R(this.gltf),this.params.origin||(this.params.origin=function(e){if((0,d.i)(e.transform))return e.transform.getOriginPoint(e.spatialReference);const t=e.extent.xmax-e.extent.width/2,s=e.extent.ymax-e.extent.height/2,i=e.extent.zmin;return new l.Z({x:t,y:s,z:i,spatialReference:e.extent.spatialReference})}(e));const o=(0,y.c)(e.vertexAttributes,e.transform,this.params.origin,{geographic:this.params.geographic,unit:"meters"});!function(e,t){if(e.components)for(const s of e.components)s.faces&&"smooth"===s.shading&&C(s,t)}(e,o),this.flipYZAxis(o);const h=r.addBufferView(5126,a.VEC3,n.ARRAY_BUFFER);let c,f,u,p;o.normal&&(c=r.addBufferView(5126,a.VEC3,n.ARRAY_BUFFER)),e.vertexAttributes.uv&&(f=r.addBufferView(5126,a.VEC2,n.ARRAY_BUFFER)),o.tangent&&(u=r.addBufferView(5126,a.VEC4,n.ARRAY_BUFFER)),e.vertexAttributes.color&&(p=r.addBufferView(5121,a.VEC4,n.ARRAY_BUFFER)),h.startAccessor("POSITION"),c&&c.startAccessor("NORMAL"),f&&f.startAccessor("TEXCOORD_0"),u&&u.startAccessor("TANGENT"),p&&p.startAccessor("COLOR_0");const g=o.position.length/3,{position:m,normal:w,tangent:b}=o,{color:A,uv:x}=e.vertexAttributes;for(let e=0;e<g;++e)h.push(m[3*e+0]),h.push(m[3*e+1]),h.push(m[3*e+2]),c&&(0,d.i)(w)&&(c.push(w[3*e+0]),c.push(w[3*e+1]),c.push(w[3*e+2])),f&&(0,d.i)(x)&&(f.push(x[2*e+0]),f.push(x[2*e+1])),u&&(0,d.i)(b)&&(u.push(b[4*e+0]),u.push(b[4*e+1]),u.push(b[4*e+2]),u.push(b[4*e+3])),p&&(0,d.i)(A)&&(p.push(A[4*e+0]),p.push(A[4*e+1]),p.push(A[4*e+2]),p.push(A[4*e+3]));const E=h.endAccessor(),T=this.addAccessor(h.index,E);let B,O,M,L,S;if(c){const e=c.endAccessor();B=this.addAccessor(c.index,e)}if(f){const e=f.endAccessor();O=this.addAccessor(f.index,e)}if(u){const e=u.endAccessor();M=this.addAccessor(u.index,e)}if(p){const e=p.endAccessor();L=this.addAccessor(p.index,e)}e.components&&e.components.length>0&&e.components[0].faces?(S=r.addBufferView(5125,a.SCALAR,n.ELEMENT_ARRAY_BUFFER),this.addMeshVertexIndexed(S,e.components,t,T,B,O,M,L)):this.addMeshVertexNonIndexed(e.components,t,T,B,O,M,L),h.finalize(),c&&c.finalize(),f&&f.finalize(),u&&u.finalize(),S&&S.finalize(),p&&p.finalize(),s||r.finalize();const I=this.gltf.meshes.length;return this.gltf.meshes.push(t),I}flipYZAxis({position:e,normal:t,tangent:s}){this.flipYZBuffer(e,3),this.flipYZBuffer(t,3),this.flipYZBuffer(s,4)}flipYZBuffer(e,t){if(!(0,d.b)(e))for(let s=1,i=2;s<e.length;s+=t,i+=t){const t=e[s],r=e[i];e[s]=r,e[i]=-t}}addMaterial(e){if(null===e)return;const t=this.materialMap.indexOf(e);if(-1!==t)return t;this.gltf.materials||(this.gltf.materials=[]);const s={};switch(e.alphaMode){case"mask":s.alphaMode=h.MASK;break;case"auto":case"blend":s.alphaMode=h.BLEND}.5!==e.alphaCutoff&&(s.alphaCutoff=e.alphaCutoff),e.doubleSided&&(s.doubleSided=e.doubleSided),s.pbrMetallicRoughness={};const i=e=>e**2.1,r=e=>{const t=e.toRgba();return t[0]=i(t[0]/255),t[1]=i(t[1]/255),t[2]=i(t[2]/255),t};if((0,d.i)(e.color)&&(s.pbrMetallicRoughness.baseColorFactor=r(e.color)),(0,d.i)(e.colorTexture)&&(s.pbrMetallicRoughness.baseColorTexture={index:this.addTexture(e.colorTexture)}),(0,d.i)(e.normalTexture)&&(s.normalTexture={index:this.addTexture(e.normalTexture)}),e instanceof b.Z){if((0,d.i)(e.emissiveTexture)&&(s.emissiveTexture={index:this.addTexture(e.emissiveTexture)}),(0,d.i)(e.emissiveColor)){const t=r(e.emissiveColor);s.emissiveFactor=[t[0],t[1],t[2]]}(0,d.i)(e.occlusionTexture)&&(s.occlusionTexture={index:this.addTexture(e.occlusionTexture)}),(0,d.i)(e.metallicRoughnessTexture)&&(s.pbrMetallicRoughness.metallicRoughnessTexture={index:this.addTexture(e.metallicRoughnessTexture)}),s.pbrMetallicRoughness.metallicFactor=e.metallic,s.pbrMetallicRoughness.roughnessFactor=e.roughness}else s.pbrMetallicRoughness.metallicFactor=1,s.pbrMetallicRoughness.roughnessFactor=1;const n=this.gltf.materials.length;return this.gltf.materials.push(s),this.materialMap.push(e),n}addTexture(e){return this.gltf.textures||(this.gltf.textures=[]),(0,p.o)(this.textureMap,e,(()=>{const t={sampler:this.addSampler(e),source:this.addImage(e)},s=this.gltf.textures.length;return this.gltf.textures.push(t),s}))}addImage(e){const t=this.imageMap.get(e);if(null!=t)return t;this.gltf.images||(this.gltf.images=[]);const s={};if(e.url)s.uri=e.url;else{s.extras=e.data;for(let t=0;t<this.gltf.images.length;++t)if(e.data===this.gltf.images[t].extras)return t;switch(this.gltf.extras.options.imageOutputType){case r.GLB:{const t=this.gltf.extras.binChunkBuffer.addBufferView(5121,a.SCALAR),i=S(e.data).then((({data:e,type:t})=>(s.mimeType=t,e)));t.writeAsync(i).then((()=>{t.finalize()})),s.bufferView=t.index;break}case r.DataURI:s.uri=function(e){const t=I(e);return(0,d.i)(t)?t.toDataURL():""}(e.data);break;default:this.gltf.extras.promises.push(S(e.data).then((({data:e,type:t})=>{s.uri=e,s.mimeType=t})))}}const i=this.gltf.images.length;return this.gltf.images.push(s),this.imageMap.set(e,i),i}addSampler(e){this.gltf.samplers||(this.gltf.samplers=[]);let t=10497,s=10497;if("string"==typeof e.wrap)switch(e.wrap){case"clamp":t=33071,s=33071;break;case"mirror":t=33648,s=33648}else{switch(e.wrap.vertical){case"clamp":s=33071;break;case"mirror":s=33648}switch(e.wrap.horizontal){case"clamp":t=33071;break;case"mirror":t=33648}}const i={wrapS:t,wrapT:s};for(let e=0;e<this.gltf.samplers.length;++e)if(JSON.stringify(i)===JSON.stringify(this.gltf.samplers[e]))return e;const r=this.gltf.samplers.length;return this.gltf.samplers.push(i),r}addAccessor(e,t){this.gltf.accessors||(this.gltf.accessors=[]);const s={bufferView:e,byteOffset:t.byteOffset,componentType:t.componentType,count:t.count,type:t.type,min:t.min,max:t.max,name:t.name};t.normalized&&(s.normalized=!0);const i=this.gltf.accessors.length;return this.gltf.accessors.push(s),i}addMeshVertexIndexed(e,t,s,i,r,n,a,o){for(const h of t){e.startAccessor("INDICES");for(let t=0;t<h.faces.length;++t)e.push(h.faces[t]);const t=e.endAccessor(),c={attributes:{POSITION:i},indices:this.addAccessor(e.index,t),material:this.addMaterial(h.material)};r&&"flat"!==h.shading&&(c.attributes.NORMAL=r),n&&(c.attributes.TEXCOORD_0=n),a&&"flat"!==h.shading&&(c.attributes.TANGENT=a),o&&(c.attributes.COLOR_0=o),s.primitives.push(c)}}addMeshVertexNonIndexed(e,t,s,i,r,n,a){const o={attributes:{POSITION:s}};i&&(o.attributes.NORMAL=i),r&&(o.attributes.TEXCOORD_0=r),n&&(o.attributes.TANGENT=n),a&&(o.attributes.COLOR_0=a),e&&(o.material=this.addMaterial(e[0].material)),t.primitives.push(o)}}class N{constructor(){this.copyright="",this.defaultScene=0,this.generator="",this._scenes=[]}addScene(e){if(this._scenes.indexOf(e)>=0)throw new Error("Scene already added");this._scenes.push(e)}removeScene(e){const t=this._scenes.indexOf(e);t>=0&&this._scenes.splice(t,1)}forEachScene(e){this._scenes.forEach(e)}}class V{constructor(){this.name="",this.nodes=[]}addNode(e){if(this.nodes.indexOf(e)>=0)throw new Error("Node already added");this.nodes.push(e)}forEachNode(e){this.nodes.forEach(e)}}class F{constructor(e){this.mesh=e,this.name="",this.translation=(0,w.b)(),this.rotation=(0,m.a)(),this.scale=(0,w.q)(w.O),this.nodes=[]}addNode(e){if(this.nodes.indexOf(e)>=0)throw new Error("Node already added");this.nodes.push(e)}forEachNode(e){this.nodes.forEach(e)}set rotationAngles(e){(0,g.f)(this.rotation,e[0],e[1],e[2])}}class _{constructor(e,t){this.file={type:"model/gltf-binary",data:e},this.origin=t}buffer(){return Promise.resolve(this.file)}download(e){return new Promise((()=>{const t=new Blob([this.file.data],{type:this.file.type});let s=e;if(s||(s="model.glb"),"glb"!==s.split(".").pop()&&(s+=".glb"),window.navigator.msSaveOrOpenBlob)window.navigator.msSaveOrOpenBlob(t,s);else{const e=document.createElement("a"),i=URL.createObjectURL(t);e.href=i,e.download=s,document.body.appendChild(e),e.click(),setTimeout((function(){document.body.removeChild(e),window.URL.revokeObjectURL(i)}),0)}}))}}function U(e,t){const s=new N,n=new V;s.addScene(n);const a=new F(e);return n.addNode(a),function(e,t){return function(e,t,s){const n=new z(e,t=t||{},s);let a=n.params;a?a.origin||(a.origin=new l.Z({x:-1,y:-1,z:-1})):a={origin:new l.Z({x:-1,y:-1,z:-1})};const o=a.origin,h=n.gltf,c=h.extras.promises;let f=1,d=1,p=null;return(0,u.as)(c).then((()=>{const e={origin:o};delete h.extras;const s="number"==typeof t.jsonSpacing?t.jsonSpacing:4,n=JSON.stringify(h,((s,n)=>{if("extras"!==s){if(n instanceof ArrayBuffer){if(function(e){if(e.byteLength<8)return!1;const t=new Uint8Array(e);return 137===t[0]&&80===t[1]&&78===t[2]&&71===t[3]&&13===t[4]&&10===t[5]&&26===t[6]&&10===t[7]}(n))switch(t.imageOutputType){case r.DataURI:case r.GLB:break;case r.External:default:{const t=`img${d}.png`;return d++,e[t]=n,t}}switch(t.bufferOutputType){case i.DataURI:return function(e){const t=[],s=new Uint8Array(e);for(let e=0;e<s.length;e++)t.push(String.fromCharCode(s[e]));return"data:application/octet-stream;base64,"+btoa(t.join(""))}(n);case i.GLB:if(p)throw new Error("Already encountered an ArrayBuffer, there should only be one in the GLB format.");return void(p=n);case i.External:default:{const t=`data${f}.bin`;return f++,e[t]=n,t}}}return n}}),s);return t.bufferOutputType===i.GLB||t.imageOutputType===r.GLB?e["model.glb"]=new E(n,p).buffer:e["model.gltf"]=n,e}))}(e,{bufferOutputType:i.GLB,imageOutputType:r.GLB,jsonSpacing:0},t)}(s,t).then((e=>new _(e["model.glb"],e.origin)))}}}]);