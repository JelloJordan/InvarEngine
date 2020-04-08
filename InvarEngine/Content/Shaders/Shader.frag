#version 330 core

in vec2 pass_textureCoords;
in vec3 surfaceNormal;
in vec3 toLightVector;
in float lightDistance;wa
//in vec3 toCameraVector;

out vec4 out_Color;

uniform sampler2D textureSampler;
uniform float lightStrength;
uniform float directionalLightStrength;
uniform vec3 directionalLightVector;
uniform float ambientLightIntensity;

uniform float shineDamper;
uniform float reflectivity;

void main(void)
{
    vec3 unitNormal = normalize(surfaceNormal);
    vec3 unitLightVector = normalize(toLightVector);

    float nDot1 = dot(unitNormal, unitLightVector);
    float sunDot = dot(unitNormal, directionalLightVector);
    float brightness = max(max(nDot1, ambientLightIntensity) * lightStrength * lightDistance, (max(sunDot, ambientLightIntensity) * directionalLightStrength));

    //vec3 unitVectorToCamera = normalize(toCameraVector);
    //vec3 lightDirection = -directionalLightVector;
    //vec3 reflectedLightDirection = reflect(lightDirection, unitNormal);

    //float specularFactor = dot(reflectedLightDirection, unitVectorToCamera);
    //specularFactor = max(specularFactor, 0);
    //float dampedFactor = pow(specularFactor, shineDamper);
    //float finalSpecular = dampedFactor * directionalLightStrength;

    out_Color = vec4(brightness,brightness,brightness,1) * texture(textureSampler, pass_textureCoords);

    //out_Color = vec4(brightness,brightness,brightness,1) * texture(textureSampler, pass_textureCoords) + vec4(finalSpecular,finalSpecular,finalSpecular, 1f);

}