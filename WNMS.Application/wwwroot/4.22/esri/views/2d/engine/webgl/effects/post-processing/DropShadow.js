// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../../../../core/screenUtils ../../../../../webgl/BufferObject ../../../../../webgl/FramebufferObject ../../../../../../chunks/_rollupPluginBabelHelpers ../../../../../../core/has ../../../../../webgl/checkWebGLError ../../../../../webgl/enums ../../../../../../chunks/builtins ../../../../../webgl/Texture ../../../../../webgl/VertexArrayObject ../../VertexStream".split(" "),function(n,p,q,r,I,J,K,L,M,A,N,B){const C=[1,0],D=[0,1];q=function(){function t(){this._verticalBlurFBO=
this._horizontalBlurFBO=null;this._size=[0,0];this._programDesc={blur:{vsPath:"post-processing/drop-shadow",fsPath:"post-processing/blur/gaussianBlur",attributes:new Map([["a_position",0]])},composite:{vsPath:"post-processing/pp",fsPath:"post-processing/drop-shadow/composite",attributes:new Map([["a_position",0]])},blit:{vsPath:"post-processing/pp",fsPath:"post-processing/blit",attributes:new Map([["a_position",0]])}}}var m=t.prototype;m.dispose=function(){this._layerFBOTexture&&(this._layerFBOTexture.dispose(),
this._layerFBOTexture=null);this._horizontalBlurFBO&&(this._horizontalBlurFBO.dispose(),this._horizontalBlurFBO=null);this._verticalBlurFBO&&(this._verticalBlurFBO.dispose(),this._verticalBlurFBO=null)};m.draw=function(b,d,e){const {context:a,state:u,painter:E}=b,{materialManager:v}=E,w=this._programDesc,h=d.width,k=d.height,l=[Math.round(h/2),Math.round(k/2)],{blurRadius:x,offsetX:F,offsetY:G,color:f}=e,H=[p.pt2px(F/2),p.pt2px(G/2)];this._createOrResizeResources(b,h,k,l);const y=this._horizontalBlurFBO;
e=this._verticalBlurFBO;a.setStencilWriteMask(0);a.setStencilTestEnabled(!1);a.setDepthWriteEnabled(!1);a.setDepthTestEnabled(!1);const z=this._layerFBOTexture;d.copyToTexture(0,0,h,k,0,0,z);this._quad||(this._quad=new B(a,[-1,-1,1,-1,-1,1,1,1]));a.setViewport(0,0,l[0],l[1]);const g=this._quad;g.bind();a.setBlendingEnabled(!1);const c=v.getProgram(b,w.blur,[{name:"radius",value:Math.ceil(x)}]);a.useProgram(c);a.bindFramebuffer(y);a.bindTexture(d.colorTexture,4);c.setUniformMatrix3fv("u_displayViewMat3",
u.displayMat3);c.setUniform2fv("u_offset",H);c.setUniform1i("u_colorTexture",4);c.setUniform2fv("u_texSize",l);c.setUniform2fv("u_direction",C);c.setUniform1f("u_sigma",x);g.draw();a.bindFramebuffer(e);a.bindTexture(y.colorTexture,5);c.setUniformMatrix3fv("u_displayViewMat3",u.displayMat3);c.setUniform2fv("u_offset",[0,0]);c.setUniform1i("u_colorTexture",5);c.setUniform2fv("u_direction",D);g.draw();a.bindFramebuffer(d);a.setViewport(0,0,h,k);b=v.getProgram(b,w.composite);a.useProgram(b);a.bindTexture(e.colorTexture,
2);b.setUniform1i("u_blurTexture",2);a.bindTexture(z,3);b.setUniform1i("u_layerFBOTexture",3);b.setUniform4fv("u_shadowColor",[f[0]/255*f[3],f[1]/255*f[3],f[2]/255*f[3],f[3]]);g.draw();a.setBlendingEnabled(!0);a.setStencilTestEnabled(!0);a.setBlendFunction(1,771);g.unbind()};m._createOrResizeResources=function(b,d,e,a){({context:b}=b);this._horizontalBlurFBO&&this._size[0]===d&&this._size[1]===e||(this._size[0]=d,this._size[1]=e,this._horizontalBlurFBO?this._horizontalBlurFBO.resize(a[0],a[1]):this._horizontalBlurFBO=
new r(b,{colorTarget:0,depthStencilTarget:0,width:a[0],height:a[1]},{target:3553,pixelFormat:6408,internalFormat:6408,dataType:5121,wrapMode:33071,samplingMode:9729,flipped:!1,width:a[0],height:a[1]}),this._verticalBlurFBO?this._verticalBlurFBO.resize(a[0],a[1]):this._verticalBlurFBO=new r(b,{colorTarget:0,depthStencilTarget:0,width:a[0],height:a[1]},{target:3553,pixelFormat:6408,internalFormat:6408,dataType:5121,wrapMode:33071,samplingMode:9729,flipped:!1,width:a[0],height:a[1]}),this._layerFBOTexture?
this._layerFBOTexture.resize(d,e):this._layerFBOTexture=new A(b,{target:3553,pixelFormat:6408,internalFormat:6408,dataType:5121,wrapMode:33071,samplingMode:9729,flipped:!1,width:d,height:e}))};return t}();n.DropShadow=q;Object.defineProperty(n,"__esModule",{value:!0})});