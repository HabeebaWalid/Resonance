# Resonance

Created in Unity, Resonance is a poetic, non-verbal journey from the depths of isolation to a place of renewal. The film relies on both visual storytelling and sound to explore complex emotional states, making its message of hope and recovery accessible across cultural and linguistic boundaries.

The narrative follows a young girl’s transformation as she moves from a fog-shrouded, grayscale world into a vibrant, living landscape awakened by the sound of music.

---

## Technical & Artistic Implementation

Resonance was built on the principle that real-time rendering technology can be a powerful medium for meaningful, emotionally rich storytelling. Each technical choice was made to serve the narrative and externalize the protagonist's internal healing process.

### A Shader-Driven Narrative

The film's central visual metaphor (the transition from grayscale to color) is driven by custom shaders that directly reflect the protagonist's emotional state. A hybrid development approach was used, combining HLSL for performance-critical effects and Unity's Shader Graph for rapid artistic iteration.

- **Grayscale & Volumetric Fog:** The initial state of isolation is represented by a world devoid of color, rendered in grayscale and shrouded in volumetric fog. These effects visually communicate a sense of emotional numbness.

- **The Bloom Of Color & Light:** As music is introduced, the grayscale shader recedes, allowing a vibrant color palette to wash over the world. This transition is accompanied by glow and bloom effects, representing the return of life, hope, and feeling.

- **A Living World:** The environment itself comes alive through shaders. Foliage, from trees, is animated with a wind-driven shader to create a sense of a living, breathing world. Interactive elements, such as a trail of flowers that bloom as the protagonist walks between them, reinforce her positive impact on her surroundings as she heals.

### Expressive Non-Verbal Animation

With no dialogue, the emotional weight of the story is carried by the character's performance.

- **Subtle Facial Animation:** A combination of blendshapes and a custom ExpressionController script was developed to achieve a range of subtle, expressive facial animations. This allowed for a nuanced performance that could communicate complex emotions without words.

- **Cinematics & Pacing:** Unity's Timeline was used to orchestrate all character animation, camera work, and the precise timing of visual effects, ensuring a cohesive and emotionally resonant cinematic experience.

---

## Conceptual Foundation

The use of nature and music in Resonance is grounded in psychological research on their therapeutic impact. The film serves as a symbolic representation of evidence-based tools for emotional restoration, aiming to contribute to ongoing conversations about mental health in a gentle, reflective manner.

## Engine & Pipeline

- **Unity Version:** 6000.0.41f1
- **Render Pipeline:** Universal Render Pipeline (URP), chosen for its flexibility in creating stylized visuals with real-time performance.

## Getting Started

To open and explore this project, you will need:

- **Unity Editor Version:** 6000.0.41f1 or newer.
- **Git LFS:** This project uses Git Large File Storage. Make sure you have it installed before cloning.

**Clone Command:**
```bash
git lfs clone https://github.com/HabeebaWalid/Resonance.git
```