"use client"

import React, { useState, useEffect } from "react";
import SideBar from "../sidebar/page";
import axios from "axios";



export default function Page() {
    const [services, setServices] = useState<Service[]>([]);
    const [departments, setDepartments] = useState<Department[]>([]);
    const [title, setTitle] = useState('');
    const [departmentID, setDepartmentID] = useState('');
    const [filterDepartmentID, setFilterDepartmentID] = useState('');
    const [isDataFetched, setIsDataFetched] = useState(false);

    const fetchServices = async (departmentID: string) => {
        try {
            let url = `http://localhost:5236/api/services`;
            if (departmentID !== "") {
                url = `http://localhost:5236/api/services/by-department/${departmentID}`;
            }

            const servicesRes = await fetch(url, {
                cache: 'no-store'
            });
            const servicesData = await servicesRes.json();
            setServices(servicesData);
        } catch (error) {
            console.error('Erreur lors de la récupération des services :', error);
        }
    };

    const fetchDepartments = async () => {
        try {
            const departmentsRes = await fetch(`http://localhost:5236/api/departments`, {
                cache: 'no-store'
            });
            const departmentsData = await departmentsRes.json();
            setDepartments(departmentsData);
        } catch (error) {
            console.error('Erreur lors de la récupération des départements :', error);
        }
    };

    const handleTitleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(e.target.value);
    };

    const handleDepartmentIDChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setDepartmentID(e.target.value);
    };

    const handleFilterDepartmentIDChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedDepartmentID = e.target.value;
        setFilterDepartmentID(selectedDepartmentID);
        fetchServices(selectedDepartmentID);
    };

    const handleSubmit = async () => {
        try {
            await axios.post('http://localhost:5236/api/services', {
                title: title,
                department_id: departmentID,
            } , {withCredentials : true});

            fetchServices(filterDepartmentID);
            setTitle('');
            setDepartmentID('');

            console.log("Service ajouté avec succès !");
        } catch (error) {
            console.error('Erreur lors de l\'ajout du service :', error);
        }
    };

    useEffect(() => {
        fetchServices(filterDepartmentID);
    }, [filterDepartmentID]);

    useEffect(() => {
        fetchDepartments();
    }, []);

    return (
        <div>
            <SideBar />
            <section className="bg-gray-50 dark:bg-gray-900 py-4 md:h-screen">
                <div className="flex flex-col sm:ml-64 px-6 lg:py-0 dark:text-white ">
                    <section>
                        <div>
                            <label htmlFor="title">Titre :</label>
                            <input
                                type="text"
                                name="title"
                                id="title"
                                value={title}
                                onChange={handleTitleChange}
                                className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                            />
                        </div>
                        <div className="mt-3">
                            <label htmlFor="departmentID">Titre du Département :</label>
                            <select
                                name="departmentID"
                                id="departmentID"
                                value={departmentID}
                                onChange={handleDepartmentIDChange}
                                className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                            >
                                <option value="">Sélectionner un département</option>
                                {departments.map((department) => (
                                    <option key={department.id} value={department.id}>
                                        {department.title}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div>
                            <button
                                className="mt-4 px-4 py-2 font-bold text-white bg-blue-500 rounded hover:bg-blue-700"
                                onClick={handleSubmit}
                            >
                                Ajouter
                            </button>
                        </div>
                    </section>
                    <section className="mt-8">
                        <select
                            name="filterDepartmentID"
                            id="filterDepartmentID"
                            value={filterDepartmentID}
                            onChange={handleFilterDepartmentIDChange}
                            className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                        >
                            <option value="">Tous les départements</option>
                            {departments.map((department) => (
                                <option key={department.id} value={department.id}>
                                    {department.title}
                                </option>
                            ))}
                        </select>
                    </section>
                    <section className="mt-8">
                        <table className="min-w-full bg-white dark:bg-gray-800">
                            <thead>
                                <tr className="text-center">
                                    <td className="py-2 ">Titre</td>
                                    <td className="py-2 ">Titre du Département</td>
                                </tr>
                            </thead>
                            <tbody>
                                {services.map((service) => (
                                    <tr key={service.id} className="text-center">
                                        <td className="py-2 ">{service.title}</td>
                                        <td className="py-2 ">{service.departmentTitle}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </section>
                </div>
            </section>
        </div>
    );
}
