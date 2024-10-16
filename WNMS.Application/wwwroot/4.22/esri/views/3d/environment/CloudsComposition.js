// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../../../core/mathUtils ../../../core/maybe ../../../chunks/mat4 ../../../chunks/mat4f64 ../../../chunks/vec3 ../../../chunks/vec3f64 ../../../geometry/support/axisAngle ./CloudsCompositionTechnique ../webgl-engine/lib/glUtil3D".split(" "),function(A,v,w,x,p,e,q,u,C,D){var f;(function(b){b[b.FINISHED=0]="FINISHED";b[b.CHANGE_ANCHOR=1]="CHANGE_ANCHOR";b[b.FADE_IN=2]="FADE_IN"})(f||(f={}));var d;(function(b){b[b.FINISHED=0]="FINISHED";b[b.FADE_OUT=1]="FADE_OUT";b[b.FADE_IN=2]="FADE_IN"})(d||
(d={}));var g;(function(b){b[b.FINISHED=0]="FINISHED";b[b.CROSS_FADE=1]="CROSS_FADE"})(g||(g={}));var n;(function(b){b[b.FINISHED=0]="FINISHED";b[b.HEIGHT_FADE=1]="HEIGHT_FADE"})(n||(n={}));let G=function(){function b(a,c,r,k){this._viewingMode=c;this._inverseProjectionMatrix=p.create();this._inverseViewMatrix=p.create();this._cameraPositionLastFrame=q.create();this._technique=new C.CloudsCompositionTechnique({rctx:a,viewingMode:this._viewingMode},null);this._vao=D.createQuadVAO(a);this._setDefaultParallax(r,
k)}var h=b.prototype;h.destroy=function(){this._technique=w.releaseMaybe(this._technique);this._vao=w.disposeMaybe(this._vao)};h.render=function(a,c,r,k,E){const y=a.camera;if(w.isNone(this._vao)||w.isNone(y))return!1;const z=a.rctx,l=this._technique.program;z.useProgram(l);this._technique.bindPipelineState(z);this._isCameraAnimating=r;a.scenelightingData.setLightDirectionUniform(l);a.scenelightingData.setUniforms(l);x.invert(this._inverseProjectionMatrix,y.projectionMatrix);x.invert(this._inverseViewMatrix,
y.viewMatrix);l.setUniformMatrix4fv("inverseProjectionMatrix",this._inverseProjectionMatrix);l.setUniformMatrix4fv("inverseViewMatrix",this._inverseViewMatrix);l.bindTexture(c.colorTexture,"cubeMap");l.setUniform2fv("cloudVariables",[k,E]);this._setParallaxParams(y.eye);this._bindParallaxParams(l,a.camera);z.bindVAO(this._vao);l.assertCompatibleVertexAttributeLocations(this._vao);z.drawArrays(5,0,4);return!0};h.isFading=function(){return this._crossFadeParams.crossFadeStage!==g.FINISHED||this._fadeInOutParams.fadeInOutStage!==
d.FINISHED||this._fadeInParams.fadeInStage!==f.FINISHED||this._fadeInOutHeightParams.fadeHeightStage!==n.FINISHED};h._setDefaultParallax=function(a,c){this._parallaxParams={anchorPointClouds:q.create(),radius:a,cloudsHeight:1E5,radiusCurvatureCorrectionFactor:0,transform:p.create()};this._parallaxParamsNew={anchorPointClouds:q.create(),radius:a,cloudsHeight:1E5,radiusCurvatureCorrectionFactor:0,transform:p.create()};this._crossFadeParams={crossFadeStage:g.FINISHED,crossFadeFactor:0,distanceThresholdFactor:.3};
this._fadeInOutParams={fadeInOutStage:d.FINISHED,fadeInOutFactor:0,distanceThresholdFactor:.6};this._fadeInParams={fadeInStage:f.FINISHED,fadeInFactor:0,distanceThresholdFactor:2};this._fadeInOutHeightParams={fadeHeightStage:n.FINISHED,fadeHeightFactor:1E4<c?1:0,heightThresholdFactor:1E4}};h._isCameraPositionFinal=function(a){return e.equals(this._cameraPositionLastFrame,a)};h._setFadeInStage=function(a){a=this._isCameraPositionFinal(a);this._fadeInParams.fadeInStage===f.FINISHED&&(this._fadeInParams.fadeInFactor=
1,e.copy(this._parallaxParams.anchorPointClouds,m),this._fadeInParams.fadeInStage=f.CHANGE_ANCHOR,this._crossFadeParams.crossFadeStage=g.FINISHED,this._fadeInOutParams.fadeInOutStage=d.FINISHED);this._fadeInParams.fadeInStage===f.CHANGE_ANCHOR&&a&&(e.copy(this._parallaxParams.anchorPointClouds,m),this._fadeInParams.fadeInStage=f.FADE_IN,this._startTime=performance.now());0<this._fadeInParams.fadeInFactor&&this._fadeInParams.fadeInStage===f.FADE_IN&&(this._fadeInParams.fadeInFactor=1-(performance.now()-
this._startTime)/500);0>=this._fadeInParams.fadeInFactor&&this._fadeInParams.fadeInStage===f.FADE_IN&&(this._fadeInParams.fadeInStage=f.FINISHED,this._fadeInParams.fadeInFactor=0)};h._setCrossFadingStage=function(){this._crossFadeParams.crossFadeStage===g.FINISHED&&(e.copy(this._parallaxParamsNew.anchorPointClouds,m),this._startTime=performance.now(),this._crossFadeParams.crossFadeFactor=0,this._crossFadeParams.crossFadeStage=g.CROSS_FADE);1>this._crossFadeParams.crossFadeFactor&&this._crossFadeParams.crossFadeStage===
g.CROSS_FADE&&(this._crossFadeParams.crossFadeFactor=(performance.now()-this._startTime)/500);1<=this._crossFadeParams.crossFadeFactor&&this._crossFadeParams.crossFadeStage===g.CROSS_FADE&&(this._crossFadeParams.crossFadeStage=g.FINISHED,this._crossFadeParams.crossFadeFactor=1,e.copy(this._parallaxParams.anchorPointClouds,this._parallaxParamsNew.anchorPointClouds))};h._setFadeInOutStage=function(a){this._fadeInOutParams.fadeInOutStage===d.FINISHED&&(this._startTime=performance.now(),this._fadeInOutParams.fadeInOutFactor=
0,this._fadeInOutParams.fadeInOutStage=d.FADE_OUT);1>this._fadeInOutParams.fadeInOutFactor&&this._fadeInOutParams.fadeInOutStage===d.FADE_OUT&&(this._fadeInOutParams.fadeInOutFactor=(performance.now()-this._startTime)/250);1<=this._fadeInOutParams.fadeInOutFactor&&this._fadeInOutParams.fadeInOutStage===d.FADE_OUT&&(this._fadeInOutParams.fadeInOutFactor=1,e.copy(this._parallaxParams.anchorPointClouds,m));1<=this._fadeInOutParams.fadeInOutFactor&&this._fadeInOutParams.fadeInOutStage===d.FADE_OUT&&this._isCameraPositionFinal(a)&&
(this._startTime=performance.now(),this._fadeInOutParams.fadeInOutFactor=1,this._fadeInOutParams.fadeInOutStage=d.FADE_IN,this._crossFadeParams.crossFadeStage=g.FINISHED,this._crossFadeParams.crossFadeFactor=0);0<this._fadeInOutParams.fadeInOutFactor&&this._fadeInOutParams.fadeInOutStage===d.FADE_IN&&(this._fadeInOutParams.fadeInOutFactor=1-(performance.now()-this._startTime)/500);0>=this._fadeInOutParams.fadeInOutFactor&&this._fadeInOutParams.fadeInOutStage===d.FADE_IN&&(this._fadeInOutParams.fadeInOutStage=
d.FINISHED,this._fadeInOutParams.fadeInOutFactor=0)};h._setFadeInOutHeight=function(a){const c=performance.now();this._startTimeHeightFade=this._fadeInOutHeightParams.fadeHeightStage===n.FINISHED?c:this._startTimeHeightFade;this._fadeInOutHeightParams.fadeHeightFactor=a?this._fadeInOutHeightParams.fadeHeightFactor+(c-this._startTimeHeightFade)/500:this._fadeInOutHeightParams.fadeHeightFactor-(c-this._startTimeHeightFade)/500;this._startTimeHeightFade=c;this._fadeInOutHeightParams.fadeHeightFactor=
v.clamp(this._fadeInOutHeightParams.fadeHeightFactor,0,1);this._fadeInOutHeightParams.fadeHeightStage=n.HEIGHT_FADE};h._setParallaxParams=function(a){e.normalize(m,a);e.scale(m,m,this._parallaxParams.radius);0===this._parallaxParams.anchorPointClouds[0]&&0===this._parallaxParams.anchorPointClouds[1]&&0===this._parallaxParams.anchorPointClouds[2]&&e.copy(this._parallaxParams.anchorPointClouds,m);var c=e.length(e.subtract(F,this._parallaxParams.anchorPointClouds,m));let r=!0;c>this._fadeInParams.distanceThresholdFactor*
this._parallaxParams.cloudsHeight||this._fadeInParams.fadeInStage!==f.FINISHED?this._setFadeInStage(a):c>this._fadeInOutParams.distanceThresholdFactor*this._parallaxParams.cloudsHeight||this._fadeInOutParams.fadeInOutStage!==d.FINISHED?this._setFadeInOutStage(a):c>this._crossFadeParams.distanceThresholdFactor*this._parallaxParams.cloudsHeight&&!this._isCameraAnimating||this._crossFadeParams.crossFadeStage!==g.FINISHED?this._setCrossFadingStage():r=!1;c=e.length(a);const k=c-this._parallaxParams.radius;
(k>1.7*this._fadeInOutHeightParams.heightThresholdFactor||k<-1*this._fadeInOutHeightParams.heightThresholdFactor)&&1>this._fadeInOutHeightParams.fadeHeightFactor?this._fadeInOutHeightParams.fadeHeightFactor=1:(k>this._fadeInOutHeightParams.heightThresholdFactor||k<-.35*this._fadeInOutHeightParams.heightThresholdFactor)&&1>this._fadeInOutHeightParams.fadeHeightFactor?this._setFadeInOutHeight(!0):k<this._fadeInOutHeightParams.heightThresholdFactor&&k>-.35*this._fadeInOutHeightParams.heightThresholdFactor&&
0<this._fadeInOutHeightParams.fadeHeightFactor?this._setFadeInOutHeight(!1):this._fadeInOutHeightParams.fadeHeightStage=n.FINISHED;this._parallaxParams.radiusCurvatureCorrectionFactor=.84*Math.sqrt(Math.max(c*c-this._parallaxParams.radius*this._parallaxParams.radius,0))/c;u.fromPoints(B,this._parallaxParams.anchorPointClouds,t);this._parallaxParams.transform=p.create();x.rotate(this._parallaxParams.transform,this._parallaxParams.transform,t[3],u.axis(t));r&&(u.fromPoints(B,this._parallaxParamsNew.anchorPointClouds,
t),this._parallaxParamsNew.transform=p.create(),x.rotate(this._parallaxParamsNew.transform,this._parallaxParamsNew.transform,t[3],u.axis(t)));e.copy(this._cameraPositionLastFrame,a)};h._bindParallaxParams=function(a,c){a.setUniform1f("cloudsHeight",this._parallaxParams.cloudsHeight);a.setUniformMatrix4fv("rotationMatrixClouds",this._parallaxParams.transform);a.setUniformMatrix4fv("rotationMatrixCloudsCrossFade",this._parallaxParamsNew.transform);a.setUniform3fv("anchorPosition",this._parallaxParams.anchorPointClouds);
a.setUniform3fv("anchorPositionCrossFade",this._parallaxParamsNew.anchorPointClouds);this._fadeInOutParams.fadeInOutStage!==d.FINISHED?a.setUniform1f("totalFadeInOut",this._fadeInOutHeightParams.fadeHeightFactor+Math.max(v.clamp(this._fadeInOutParams.fadeInOutFactor,0,1))):a.setUniform1f("totalFadeInOut",this._fadeInOutHeightParams.fadeHeightFactor+Math.max(v.clamp(this._fadeInParams.fadeInFactor,0,1)));a.setUniform1f("radiusCurvatureCorrectionFactor",this._parallaxParams.radiusCurvatureCorrectionFactor);
a.setUniform1i("crossFade",this._crossFadeParams.crossFadeStage);a.setUniform1f("crossFadeAnchorFactor",v.clamp(this._crossFadeParams.crossFadeFactor,0,1));a.setUniform1f("radius",this._parallaxParams.radius);a.setUniform3fv("cameraPosition",c.eye)};return b}();const B=q.fromValues(0,0,1),t=u.create(),m=q.create(),F=q.create();A.CloudsComposition=G;Object.defineProperty(A,"__esModule",{value:!0})});