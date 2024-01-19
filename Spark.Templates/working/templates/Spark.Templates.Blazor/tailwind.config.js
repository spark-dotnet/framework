module.exports = {
    content: ["Pages/**/*.razor"],
    theme: {
        extend: {},
    },
    daisyui: {
        themes: ["light"],
    },
    plugins: [require("@tailwindcss/typography"), require("daisyui")],
}

