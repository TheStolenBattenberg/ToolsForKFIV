#version 330 core
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec4 vColour;

out vec4 fColour;

uniform mat4 cameraMatrix;

void main()
{
    gl_Position = cameraMatrix * vec4(vPosition, 1.0f);
    fColour = vColour;
}