#version 330 core
layout(location = 0) out vec4 cDiffuse;

in vec3 fNormal;
in vec2 fTexcoord;
in vec4 fColour;

uniform sampler2D sDiffuse;

void main()
{
    cDiffuse = fColour * texture(sDiffuse, fTexcoord);
}