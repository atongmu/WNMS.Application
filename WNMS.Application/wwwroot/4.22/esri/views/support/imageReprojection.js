// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../chunks/_rollupPluginBabelHelpers ../../request ../../geometry/Point ../../geometry/projection ../../layers/support/rasterFunctions/rasterProjectionHelper ../2d/engine/Bitmap ../2d/engine/webgl/VertexStream ../2d/engine/webgl/shaders/MaterialPrograms ../webgl/FramebufferObject ../webgl/ProgramCache ../webgl/rasterUtils ../webgl/RenderingContext ../webgl/Texture".split(" "),function(m,v,w,n,x,p,q,y,z,A,B,C,D,r){let F=function(){function t(a){a?(this._ownsRctx=!1,this._rctx=a):
(this._ownsRctx=!0,a=document.createElement("canvas").getContext("webgl"),a.getExtension("OES_texture_float"),this._rctx=new D.RenderingContext(a,{}));a=z.createProgramTemplate("raster/reproject","raster/reproject",new Map([["a_position",0]]));this._program=(new B(this._rctx)).getProgram(a,{applyProjection:!0,bilinear:!1,bicubic:!1});this._program.setUniform1f("u_opacity",1);this._program.setUniform1i("u_image",0);this._program.setUniform1i("u_flipY",0);this._program.setUniform1i("u_transformGrid",
1);this._quad=new y(this._rctx,[0,0,1,0,0,1,1,1])}var h=t.prototype;h.reprojectTexture=function(a,b,e=!1){const c=x.project(a.extent,b);var d=new n({x:(a.extent.xmax-a.extent.xmin)/a.texture.descriptor.width,y:(a.extent.ymax-a.extent.ymin)/a.texture.descriptor.height,spatialReference:a.extent.spatialReference});const {x:k,y:E}=p.projectResolution(d,b,a.extent);var f=(k+E)/2;b=Math.round((c.xmax-c.xmin)/f);d=Math.round((c.ymax-c.ymin)/f);f=(c.width/b+c.height/d)/2;f=new n({x:f,y:f,spatialReference:c.spatialReference});
const l=p.getProjectionOffsetGrid({projectedExtent:c,srcBufferExtent:a.extent,pixelSize:f,hasWrapAround:!0,spacing:[16,16]}),u=C.createTransformTexture(this._rctx,l);f=new r(this._rctx,{width:b,height:d,pixelFormat:6408,dataType:5121,wrapMode:33071,samplingMode:9729,hasMipmap:!1});const g=new A(this._rctx,{colorTarget:0,depthStencilTarget:0,width:b,height:d},f);this._rctx.bindFramebuffer(g);this._rctx.setViewport(0,0,b,d);this._rctx.useProgram(this._program);this._rctx.bindTexture(a.texture,0);this._rctx.bindTexture(u,
1);this._quad.bind();this._program.setUniform2f("u_srcImageSize",a.texture.descriptor.width,a.texture.descriptor.height);this._program.setUniform2fv("u_transformSpacing",l.spacing);this._program.setUniform2fv("u_transformGridSize",l.size);this._program.setUniform2f("u_targetImageSize",b,d);this._quad.draw();this._quad.unbind();this._rctx.useProgram(null);this._rctx.bindFramebuffer(null);u.dispose();if(e)return a=new ImageData(g.descriptor.width,g.descriptor.height),g.readPixels(0,0,g.descriptor.width,
g.descriptor.height,6408,5121,a.data),g.detachColorTexture(36064),g.dispose(),{texture:f,extent:c,imageData:a};g.detachColorTexture(36064);g.dispose();return{texture:f,extent:c}};h.reprojectBitmapData=function(a,b){var e=q.isImageSource(a.bitmapData)?q.rasterize(a.bitmapData):a.bitmapData;e=new r(this._rctx,{width:a.bitmapData.width,height:a.bitmapData.height,pixelFormat:6408,dataType:5121,wrapMode:33071,samplingMode:9729,hasMipmap:!1},e);a=this.reprojectTexture({texture:e,extent:a.extent},b,!0);
a.texture.dispose();b=document.createElement("canvas");b.width=a.imageData.width;b.height=a.imageData.height;b.getContext("2d").putImageData(a.imageData,0,0);return{bitmapData:b,extent:a.extent}};h.loadAndReprojectBitmapData=function(){var a=v._asyncToGenerator(function*(b,e,c){b=(yield w(b,{responseType:"image"})).data;const d=document.createElement("canvas");d.width=b.width;d.height=b.height;const k=d.getContext("2d");k.drawImage(b,0,0);b=k.getImageData(0,0,d.width,d.height);e=this.reprojectBitmapData({bitmapData:b,
extent:e},c);return{bitmapData:e.bitmapData,extent:e.extent}});return function(b,e,c){return a.apply(this,arguments)}}();h.destroy=function(){this._quad.dispose();this._program.dispose();this._ownsRctx&&this._rctx.dispose()};return t}();m.ImageReprojector=F;Object.defineProperty(m,"__esModule",{value:!0})});