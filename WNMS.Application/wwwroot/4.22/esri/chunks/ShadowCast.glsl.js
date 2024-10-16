// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define("exports ../views/3d/webgl-engine/core/shaderLibrary/ScreenSpacePass ../views/3d/webgl-engine/core/shaderLibrary/output/ReadLinearDepth.glsl ../views/3d/webgl-engine/core/shaderLibrary/shading/ReadShadowMap.glsl ../views/3d/webgl-engine/core/shaderLibrary/util/CameraSpace.glsl ../views/3d/webgl-engine/core/shaderLibrary/util/RgbaFloatEncoding.glsl ../views/3d/webgl-engine/core/shaderModules/interfaces ../views/3d/webgl-engine/core/shaderModules/ShaderBuilder".split(" "),function(e,k,l,m,n,
p,c,q){function f(d){const a=new q.ShaderBuilder;a.fragment.include(p.RgbaFloatEncoding);a.fragment.include(l.ReadLinearDepth);a.include(n.CameraSpace);a.include(k.ScreenSpacePass);var {pass:b}=d;if(1===b){const {visualization:g,bandsEnabled:h}=d;a.fragment.constants.add("inverseSampleValue","float",255);a.fragment.uniforms.add("shadowCastMap","sampler2D");a.fragment.uniforms.add("sampleScale","float");a.fragment.uniforms.add("opacityFromElevation","float");d=0===g;b=1===g;a.fragment.uniforms.add("color",
"vec4");d?h&&a.fragment.uniforms.add("bandSize","float"):b&&a.fragment.uniforms.add("threshold","float");a.fragment.code.add(c.glsl`
      void main(void) {
        vec4 record = texture2D(shadowCastMap, uv);
        float pixelSamples = record.r * inverseSampleValue;

        if (pixelSamples < 1.0) {
          discard;
        }

        float strength = pixelSamples * sampleScale;

        ${b?c.glsl`
            if (strength <= threshold) {
              discard;
            }`:""}

        ${d&&h?c.glsl`strength = ceil(strength / bandSize) * bandSize;`:""}

        gl_FragColor = vec4(color.xyz, color.a * opacityFromElevation ${d?c.glsl`* strength`:""});
      }
    `)}else if(0===b||2===b)a.include(m.ReadShadowMap),a.fragment.uniforms.add("depthMap","sampler2D"),a.fragment.uniforms.add("inverseView","mat4"),a.fragment.uniforms.add("nearFar","vec2"),0===b?a.fragment.constants.add("sampleValue","float",r):a.fragment.constants.add("shadowColor","vec4",[0,0,0,.8]),a.fragment.code.add(c.glsl`
      void main(void) {

        float depth = rgba2float(texture2D(depthMap, uv));
        // 0.0 is the clear value of depthMap, which means nothing has been drawn there and we should discard
        if (depth == 0.0) {
          discard;
        }

        float currentPixelDepth = linearDepthFromFloat(depth, nearFar);

        if (-currentPixelDepth > nearFar.y || -currentPixelDepth < nearFar.x) {
          discard;
        }

        vec4 currentPixelPos = vec4(reconstructPosition(gl_FragCoord.xy, currentPixelDepth), 1.0);
        vec4 worldSpacePos = inverseView * currentPixelPos;

        mat4 shadowMatrix;
        float linearDepth = -currentPixelDepth;
        int i = chooseCascade(linearDepth, shadowMatrix);

        if (i >= uShadowMapNum) {
          discard;
        }

        vec3 lvpos = lightSpacePosition(worldSpacePos.xyz, shadowMatrix);

        // vertex completely outside? -> no shadow
        if (lvpos.z >= 1.0 || lvpos.x < 0.0 || lvpos.x > 1.0 || lvpos.y < 0.0 || lvpos.y > 1.0) {
          discard;
        }

        vec2 uvShadow = cascadeCoordinates(i, lvpos);

        float depthShadow = readShadowMapDepth(uvShadow, uShadowMapTex);
        bool shadow = depthShadow < lvpos.z;

        if (!shadow) {
          discard;
        }

        gl_FragColor = ${0===b?c.glsl`vec4(sampleValue)`:c.glsl`shadowColor`};
      }
    `);return a}const r=1/255,t=Object.freeze({__proto__:null,shadowCastMaxSamples:255,build:f});e.ShadowCast=t;e.build=f;e.shadowCastMaxSamples=255});