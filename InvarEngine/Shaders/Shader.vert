#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 textCoord;
layout (location = 2) in vec4 vertColor;

out vec2 pass_textureCoords;

uniform mat4 Model;
uniform mat4 View;
uniform mat4 Projection;

void main(void)
{
    vec4 worldposition = vec4(position, 1.0f) * Model;
    gl_Position =  worldposition * View * Projection;

    pass_textureCoords = textCoord;
}