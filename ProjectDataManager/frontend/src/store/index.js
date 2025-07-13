import axios from "axios";
import { createStore } from "vuex";

export default createStore({
    state: {
        employees: [
            {
                id: 1,
                firstName: "TestName",
                lastname: "TestLastName",
                middleName: null,
            },
            {
                id: 2,
                firstName: "TestName2",
                lastname: "TestLastName2",
                middleName: "asd",
            }
        ],

        projectData: {
            name: "",
            customerCompany: "",
            contractorCompany: "",
            startDate: null,
            endDate: null,
            priority: 0,
            manager: "",
            executers: []
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
            return state.projectData.executers;
        },
    },

    mutations: {
        deleteExecuter(state, payload) {
            state.projectData.executers = state.projectData.executers.filter(e => e.id !== payload.id);
        },

        setEmployees() {

        }
    },

    actions: {
        loadEmployees({ commit, state }) {
            commit("setIsLoading", true);

            return axios.get("/api/Employee/GetEmployee", {
                params: {
                    term: state.term,
                    sortBy: state.sortByColumn,
                    isDescending: state.isDescending
                }
            }).then(response => {
                commit("setEmployees", response.data.contacts);
            }).finally(() => {
                commit("setIsLoading", false);
            })
        }
    },
    modules: {
    }
})
