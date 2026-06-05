/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        campus: {
          bg:      '#0f172a',
          surface: '#1e293b',
          border:  '#334155',
          accent:  '#3b82f6',
          accentHv:'#2563eb',
          text:    '#f1f5f9',
          muted:   '#94a3b8',
        }
      },
      fontFamily: {
        mono: ['"IBM Plex Mono"', 'monospace'],
      }
    }
  },
  plugins: []
}

