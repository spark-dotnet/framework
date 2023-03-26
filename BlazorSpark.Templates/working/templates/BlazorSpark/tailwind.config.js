module.exports = {
    content: ["**/*.razor", "**/*.cshtml", "**/*.html"],
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/line-clamp'),
    ],
}