import axios from "axios";
import { createStore } from "vuex";

export default createStore({
    state: {
        employees: [],
        selecteedEmployees: [],

        projectData: {
            id: 0,
            name: "",
            customerCompany: "",
            contractorCompany: "",
            startDate: null,
            endDate: null,
            priority: 0,
            projectManagerId: "",

            projectEmployeesId: []
        },

        term: "",

        sortByColumn: "",
        isDescending: false,

        isLoading: false
    },

    getters: {
        isLoading(state) {
            return state.isLoading;
        },

        project(state) {
            return state.projectData;
        },

        employees(state) {
            return state.employees;
        },

        executers(state) {
            return state.selecteedEmployees;
        },
    },

    mutations: {
        deleteExecuter(state, payload) {
            state.selecteedEmployees = state.selecteedEmployees.filter(e => e.id !== payload.id);
        },

        setEmployees(state, employees) {
            state.employees = employees;
        },

        setExecuter(state, employees) {
            state.selecteedEmployees.push(employees);
        },

        setSelectedEmployeesId(state) {
            state.projectData.projectEmployeesId = state.selecteedEmployees.map(e => e.id);
        },

        setTerm(state, value) {
            state.term = value;
        },

        setIsLoading(state, value) {
            state.isLoading = value;
        },

        setDateTime(state) {
            state.projectData.startDate = new Date(state.projectData.endDate);
            state.projectData.endDate = new Date(state.projectData.endDate);
        },

        resetFields(state) {
            state.selecteedEmployees = [];

            state.projectData = {
                id: 0,
                name: "",
                customerCompany: "",
                contractorCompany: "",
                startDate: null,
                endDate: null,
                priority: 0,
                projectManagerId: "",

                projectEmployeesId: []
            };
        }
    },

    actions: {
        loadEmployees({ commit, state }) {
            commit("setIsLoading", true);

            return axios.get("/api/Employee/GetEmployeesForSelect", {
                params: {
                    term: state.term || ""
                }
            }).then(response => {
                commit("setEmployees", response.data);
            }).finally(() => {
                commit("setIsLoading", false);
            })
        },

        createProject({ commit, state }) {
            commit("setIsLoading", true);
            commit("setSelectedEmployeesId");

            return axios.post("/api/Project/CreateProject", state.projectData)
                .finally(() => {
                    commit("setIsLoading", false);
                });
        },
    }
})
