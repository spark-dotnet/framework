module.exports = {
    content: ["Pages/**/*.cshtml"],
    theme: {
        extend: {},
    },
    daisyui: {
        themes: ["light"],
    },
    plugins: [require("@tailwindcss/typography"), require("daisyui")],
}

