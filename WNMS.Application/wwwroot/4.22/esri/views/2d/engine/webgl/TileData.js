// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../core/maybe ./MemoryRequirements ./TileBufferData ./TileDisplayData ./Utils ./WGLDisplayRecord ./cpuMapped/DisplayRecordReader ./mesh/VertexBuffer ./mesh/VertexVector ./util/Writer".split(" "),function(z,B,t,w,x,u,A,C,D,E,F){const v=new t,n=new t;t=function(){function l(){this.tileBufferData=this.tileDisplayData=null}var q=l.prototype;q.reshuffle=function(){v.reset();const b=x.groupRecordsByGeometryType(this.tileDisplayData.displayObjects);for(var c of b)for(var a of c)a&&
v.needMore(a.geometryType,a.meshData?a.meshData.vertexCount:a.vertexCount,a.meshData?a.meshData.indexData.length:a.indexCount);c=b.length;a=new w;for(var d=0;d<c;++d){a.geometries[d].indexBuffer=new Uint32Array(Math.round(1.5*v.indicesFor(d)));var e=[];for(var f in this.tileBufferData.geometries[d].vertexBuffer)e.push(this.tileBufferData.geometries[d].vertexBuffer[f].stride);e=l._computeVertexAlignment(e);var h=Math.round(1.5*v.verticesFor(d));e=l._align(h,e);for(var g in this.tileBufferData.geometries[d].vertexBuffer)h=
this.tileBufferData.geometries[d].vertexBuffer[g].stride,a.geometries[d].vertexBuffer[g]={stride:h,data:u.allocateTypedArrayBuffer(e,h)}}n.reset();this.tileDisplayData.displayList.clear();for(f=0;f<c;++f){g=b[f];for(const k of g){if(k.meshData)k.writeMeshDataToBuffers(n.verticesFor(f),a.geometries[f].vertexBuffer,n.indicesFor(f),a.geometries[f].indexBuffer),k.meshData=null;else{g=this.tileBufferData.geometries[f].vertexBuffer;d=this.tileBufferData.geometries[f].indexBuffer;e=a.geometries[f].vertexBuffer;
h=a.geometries[f].indexBuffer;const p=n.verticesFor(f),m=n.indicesFor(f);u.copyMeshData(p,m,e,h,k,g,d);k.vertexFrom=p;k.indexFrom=m}n.needMore(f,k.vertexCount,k.indexCount)}}for(const k of this.tileDisplayData.displayObjects)this.tileDisplayData.displayList.addToList(k.displayRecords);this.tileBufferData=a};q.getStrides=function(){const b=[];for(let c=0;c<this.tileBufferData.geometries.length;++c){const a=this.tileBufferData.geometries[c];b[c]={};for(const d in a.vertexBuffer)b[c][d]=a.vertexBuffer[d].stride}return b};
q.clone=function(){const b=new l;b.tileBufferData=this.tileBufferData.clone();b.tileDisplayData=this.tileDisplayData.clone();return b};q._guessSize=function(){const {displayObjects:b}=this.tileDisplayData,c=Math.min(b.length,4);let a=0;for(let d=0;d<c;d++)a=Math.max(a,b[d].displayRecords.length);return 2*(12*b.length+b.length*a*40)};q.serialize=function(){const b=this.tileBufferData.serialize(),c=this.tileBufferData.getBuffers(),a=this.tileDisplayData.serialize(new F(Int32Array,this._guessSize())).buffer();
c.push(a);return{result:{displayData:a,bufferData:b},transferList:c}};l.fromVertexData=function(b,c){const a={},d=new Map;for(const e of c)d.set(e.id,e);u.forEachGeometryType(e=>{const f=b.data[e];if(B.isSome(f)){const g=C.DisplayRecordReader.from(f.records).getCursor();for(;g.next();){var h=g.id;const k=g.materialKey,p=g.indexFrom,m=g.indexCount,r=g.vertexFrom,y=g.vertexCount,G=d.get(h);h=new A(h,e,k);h.indexFrom=p;h.indexCount=m;h.vertexFrom=r;h.vertexCount=y;G.displayRecords.push(h)}a[e]=D.VertexBuffers.fromVertexData(f,
e)}else a[e]=(new E.VertexVectors(e,0,{fill:"default"})).intoBuffers()});return l.fromMeshData({displayObjects:c,vertexBuffersMap:a})};l.fromMeshData=function(b){const c=new l,a=new x.default,d=new w;a.displayObjects=b.displayObjects;for(const e in b.vertexBuffersMap){const f=b.vertexBuffersMap[e];d.geometries[e].indexBuffer=f.indexBuffer;d.geometries[e].vertexBuffer=f.namedBuffers}c.tileDisplayData=a;c.tileBufferData=d;return c};l.bind=function(b,c){const a=new l;a.tileDisplayData=b;a.tileBufferData=
c;return a};l.create=function(b,c){const a=new l;a.tileDisplayData=new x.default;a.tileDisplayData.displayObjects=b;const d=[0,0,0,0,0],e=[0,0,0,0,0],f=[[],[],[],[],[]];for(var h of b)for(var g of h.displayRecords)f[g.geometryType].push(g),d[g.geometryType]+=g.meshData.vertexCount,e[g.geometryType]+=g.meshData.indexData.length;b=new w;c=[c.fill||{},c.line||{},c.icon||{},c.text||{},c.label||{}];for(h=0;5>h;h++){g=new Uint32Array(e[h]);{var k=c[h];var p=d[h];const m={};for(const r in k){const y={data:u.allocateTypedArrayBuffer(p,
k[r]),stride:k[r]};m[r]=y}k=m}A.writeAllMeshDataToBuffers(f[h],k,g);b.geometries[h]={indexBuffer:g,vertexBuffer:k}}a.tileBufferData=b;return a};l._align=function(b,c){const a=b%c;return 0===a?b:b+(c-a)};l._computeVertexAlignment=function(b){let c=!1,a=!1;for(const d of b)2===d%4?c=!0:0!==d%4&&(a=!0);return a?4:c?2:1};return l}();z.TileData=t;Object.defineProperty(z,"__esModule",{value:!0})});