import { createRouter, createWebHistory } from "vue-router";
import ProjectInfoStep from "../components/Step1_ProjectInfo.vue";

const routes = [
    {
        path: "/",
        name: "info",
        component: ProjectInfoStep
    },
    {
        path: "/companies",
        name: "companies",
        component: () => import("../components/Step2_Companies.vue")
    },
    {
        path: "/manager",
        name: "manager",
        component: () => import("../components/Step3_ProjectManager.vue")
    },
    {
        path: "/executers",
        name: "executers",
        component: () => import("../components/Step4_ProjectExecuters.vue")
    },
    {
        path: "/documents",
        name: "documents",
        component: () => import("../components/Step5_DocumentsUpload.vue")
    }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
})

export default router;
