import RoughViz from 'rough-viz/dist/roughviz.min'

export const createBarChart = (config) => new RoughViz.Bar(config)

export const createHorizontalBarChart = (config) => new RoughViz.BarH(config)