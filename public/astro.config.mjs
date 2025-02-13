// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';
import react from '@astrojs/react';

const Sidebar = [
    { 
      label: 'Feliz',
      items: [
        'feliz',
        'feliz/installation',
        'feliz/syntax',
        'feliz/reactapisupport',
        'feliz/typesafestyling',
        'feliz/typesafecss',
        'feliz/usingjsx',
        'feliz/usewithelmish',
        'feliz/contributing',
        {
          label: 'React Components',
          collapsed: true,
          items: [
            'feliz/react/statelesscomponents',
            'feliz/react/notjustfunctions',
            'feliz/react/statefulcomponents',
            'feliz/react/effectfulcomponents',
            'feliz/react/subscriptionswitheffects',
            'feliz/react/contextpropagation',
            'feliz/react/portals',
            'feliz/react/hoveranimations',
            'feliz/react/workingwithdates',
            'feliz/react/usingreferences',
            'feliz/react/commonpitfalls',
            'feliz/react/renderstatichtml',
            'feliz/react/strictmode',
            'feliz/react/codesplitting',
            'feliz/react/aliasingprop',
          ]
        }
      ]  
    },
]

// https://astro.build/config
export default defineConfig({
    integrations: [starlight({
        title: 'Feliz',
        sidebar: Sidebar,
        social: {
            github: 'https://github.com/Zaid-Ajaj/Feliz',
        }
		}), react()],
});