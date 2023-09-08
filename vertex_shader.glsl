#version 330 core

layout(location = 0) in vec3 vertexPosition;
layout(location = 1) in vec3 vertexColor;

out vec3 fragColor;

uniform mat4 model;       // Pour la transformation du modèle
uniform mat4 view;        // Pour la transformation de la caméra
uniform mat4 projection;  // Pour la projection perspective

void main()
{
    fragColor = vertexColor;
    gl_Position =  projection * view * model * vec4(vertexPosition, 1.0);
}
