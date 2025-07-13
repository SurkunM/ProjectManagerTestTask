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

        projectData: [
            {
                name: "",
                customerCompany: "",
                contractorCompany: "",
                startDate: null,
                endDate: null,
                priority: 0,
                manager: "1"
            }
        ],

        term: "",

        sortByColumn: "",
        isDescending: false,

        isLoading: false
    },

    getters: {
        project(state) {
            return state.projectData;
        },

        employees(state) {
            return state.employees;
        }
    },

    mutations: {
    },

    actions: {
        loadContacts({ commit, state }) {
            commit("setIsLoading", true);

            return axios.get("/api/PhoneBook/GetContacts", {
                params: {
                    term: state.term,
                    sortBy: state.sortByColumn,
                    isDescending: state.isDescending
                }
            }).then(response => {
                commit("setContacts", response.data.contacts);
                commit("setContactsCount", response.data.totalCount);

                commit("setSelectedCheckbox");
            }).finally(() => {
                commit("setIsLoading", false);
            })
        },
    },
    modules: {
    }
})
