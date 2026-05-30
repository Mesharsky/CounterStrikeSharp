import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";

const site = process.env.DOCS_SITE ?? "https://mesharsky.github.io";
const base = process.env.DOCS_BASE ?? "/";

// https://astro.build/config
export default defineConfig({
    site,
    base,
    trailingSlash: "ignore",
    integrations: [
        starlight({
            title: "CounterStrikeSharp",
            description: "Write Counter-Strike 2 server plugins in C#.",
            logo: {
                light: "./src/assets/cssharp.svg",
                dark: "./src/assets/cssharp.svg",
                replacesTitle: false,
            },
            favicon: "/cssharp.svg",
            customCss: ["./src/styles/custom.css"],
            social: {
                github: "https://github.com/roflmuffin/CounterStrikeSharp",
                discord: "https://discord.gg/eAZU3guKWU",
            },
            editLink: {
                baseUrl:
                    "https://github.com/roflmuffin/CounterStrikeSharp/edit/main/docs-site/",
            },
            lastUpdated: true,
            pagination: true,
            head: [
                {
                    tag: "script",
                    attrs: {
                        "data-goatcounter": "https://cssharp.goatcounter.com/count",
                        async: true,
                        src: "//gc.zgo.at/count.js",
                    },
                },
            ],
            sidebar: [
                {
                    label: "Guides",
                    items: [
                        { label: "Getting started", slug: "guides/getting-started" },
                        { label: "Hello world plugin", slug: "guides/hello-world-plugin" },
                        { label: "Upgrading", slug: "guides/upgrading" },
                        { label: "Referencing players", slug: "guides/referencing-players" },
                        { label: "Dependency injection", slug: "guides/dependency-injection" },
                        {
                            label: "Automatic build and deploy",
                            slug: "guides/auto-build-and-deploy",
                        },
                    ],
                },
                {
                    label: "Features",
                    items: [
                        { label: "Console commands", slug: "features/console-commands" },
                        { label: "Console variables", slug: "features/console-variables" },
                        { label: "Game events", slug: "features/game-events" },
                        { label: "Global listeners", slug: "features/global-listeners" },
                        { label: "Timers", slug: "features/timers" },
                        { label: "Menus", slug: "features/menus" },
                        { label: "Translations", slug: "features/translations" },
                        { label: "Shared plugin API", slug: "features/shared-plugin-api" },
                    ],
                },
                {
                    label: "Admin framework",
                    items: [
                        { label: "Defining admins", slug: "admin-framework/defining-admins" },
                        {
                            label: "Admin command attributes",
                            slug: "admin-framework/admin-command-attributes",
                        },
                        {
                            label: "Defining admin groups",
                            slug: "admin-framework/defining-admin-groups",
                        },
                        {
                            label: "Defining admin immunity",
                            slug: "admin-framework/defining-admin-immunity",
                        },
                        {
                            label: "Defining command overrides",
                            slug: "admin-framework/defining-command-overrides",
                        },
                    ],
                },
                {
                    label: "Examples",
                    autogenerate: { directory: "examples" },
                },
                {
                    label: "Reference",
                    items: [
                        { label: "Core configuration", slug: "reference/core-configuration" },
                    ],
                },
                {
                    label: "API reference",
                    link: "/api/",
                    attrs: { target: "_self" },
                },
            ],
        }),
    ],
});
