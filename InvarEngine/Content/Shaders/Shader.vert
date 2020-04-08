#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 textCoord;
layout (location = 2) in vec4 vertColor;
layout (location = 3) in vec3 normal;

out vec2 pass_textureCoords;
out vec3 surfaceNormal;
out vec3 toLightVector;
out float lightDistance;
//out vec3 toCameraVector;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform vec3 lightPosition;
uniform float lightRange;
uniform vec3 lightDirection;

void main(void)
{
    vec4 worldposition = vec4(position, 1) * Model;
    gl_Position =  worldposition * View * Projection;
    pass_textureCoords = textCoord;

    surfaceNormal = (vec4(normal, 0) * Model).xyz;
    toLightVector = lightPosition - worldposition.xyz;
    lightDistance = 1f - max(0, (distance(lightPosition, worldposition.xyz)/lightRange));
     

    //toCameraVector = (inverse(View) * vec4(position, 1)).xyz - worldposition.xyz;
    //toCameraVector = position - (Model * vec4(0, 0, 0, 1).xyz).xyz
    //toCameraVector = (inverse(View) * vec4(0, 0, 0, 1)).xyz;
    //toCameraVector = (View * vec4(position, 1).xyz).xyz;
}