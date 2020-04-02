#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 textCoord;
layout (location = 2) in vec4 vertColor;
layout (location = 3) in vec3 normal;

out vec2 pass_textureCoords;
out vec3 surfaceNormal;
out vec3 toLightVector;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;
uniform vec3 lightPosition;

void main(void)
{
    vec4 worldposition = vec4(position, 1) * Model;
    gl_Position =  worldposition * View * Projection;
    pass_textureCoords = textCoord;

    surfaceNormal = (Model * vec4(normal, 0)).xyz;
    toLightVector = lightPosition - position;
}