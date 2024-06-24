"use client"

import axios from "axios";
import { useEffect, useState } from "react";
import SideBar from "../sidebar/page";
import { motion } from 'framer-motion';

export default function Page() {
    const [title, setTitle] = useState('');
    const [newTitle, setNewTitle] = useState('');
    const [skillsGroups, setSkillsGroup] = useState<GroupOfSkills[]>([]);
    const [selectedId, setSelectedId] = useState(null)
    const [isModal, setIsModal] = useState(false)
    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        const year = date.getFullYear();
        let month: string | number = date.getMonth() + 1;
        let day: string | number = date.getDate();

        if (month < 10) {
            month = '0' + month;
        }
        if (day < 10) {
            day = '0' + day;
        }

        return `${year}-${month}-${day}`;
    };

    const fetchSkillsGroup = async () => {
        try {
            const skillsGroupRes = await axios.get(`http://localhost:5236/api/skillsGroup`);
            setSkillsGroup(skillsGroupRes.data);
        } catch (error) {
            console.error('Erreur lors de la récupération des groupes de compétences :', error);
        }
    };

    const handleDelete = async(id: any) => {
        try {
            await axios.delete(`http://localhost:5236/api/skillsGroup/${id}`, {
                withCredentials: true
            },);
            fetchSkillsGroup();
        } catch (error) {
            console.error('Erreur lors de la suppression de l\'employé :', error);
        }
    }

    const fetchSkillsGroupById = async (id: string) => {
        try {
            const response = await axios.get(`http://localhost:5236/api/skillsGroup/${id}`)
            const data = await response.data
            setNewTitle(data.title)
        } catch (error) {

        }

    }

    const handleUpdate = async (e:any ,id: any) => {
        e.preventDefault()
        try {
            await axios.put(`http://localhost:5236/api/skillsGroup/${id}`, {
                title: newTitle
            }, {
                withCredentials: true
            })
            setIsModal(false)
            fetchSkillsGroup()
        } catch (error) {
            console.log(error);

        }

    }

    const handleclick = async (id: any) => {
        const test = await fetchSkillsGroupById(id)
        setIsModal(!isModal);
        setSelectedId(id)
    }
    const handleTitleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setTitle(e.target.value);
    };
    const handleNewTitleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setNewTitle(e.target.value);
    };

    const handleSubmit = async (e: React.FormEvent<HTMLButtonElement>) => {
        e.preventDefault();
        try {
            const response = await axios.post(`http://localhost:5236/api/skillsGroup`, {
                title: title
            }, { withCredentials: true });
            setTitle('');
            fetchSkillsGroup();
        } catch (error) {
            console.log(error);

        }
    };


    useEffect(() => {
        fetchSkillsGroup();
    }, []);

    return (
        <div>
            <SideBar />
            <section className="bg-gray-50 dark:bg-gray-900 py-4 md:h-screen">
                <div className="flex flex-col sm:ml-64 px-6 lg:py-0 dark:text-white ">
                    <section>
                        <h2 className="text-xl mb-4">Ajout des groupes de compétences</h2>
                        <form>
                            <div>
                                <div>
                                    <label htmlFor="titre">Titre</label>
                                    <input
                                        type="text"
                                        name="title"
                                        id="title"
                                        value={title}
                                        onChange={handleTitleChange}
                                        className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                    />
                                </div>
                            </div>
                            <button onClick={handleSubmit} className="bg-blue-500 text-white px-4 py-2 rounded-md mt-4">
                                Ajouter
                            </button>
                        </form>
                    </section>
                    <section className="mt-8">
                        <div>
                            <h2 className="text-xl mb-4">Liste des groupes de compétences</h2>

                        </div>
                        <table className="min-w-full bg-white dark:bg-gray-800">
                            <thead>
                                <tr>
                                    <th className="py-2">Titre</th>
                                </tr>
                            </thead>
                            <tbody>
                                {skillsGroups.map((skillsGroup) => (
                                    <motion.tr
                                        key={skillsGroup.id}
                                        whileHover={{ scale: 1.02, backgroundColor: '#f0f0f0', color: "black" }}
                                        transition={{ duration: 0.2 }}
                                        className="text-center"
                                    >
                                        <td className="py-4">{skillsGroup.title}</td>
                                        <td className="py-4 flex text-center">
                                            <span className="text-lg text-danger cursor-pointer active:opacity-50" onClick={() => handleDelete(skillsGroup.id)}>
                                                <svg width="30px" height="30px" viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">

                                                    <defs>

                                                        <style>
                                                            {`
                                                    .cls-1 { fill: red; }
                                                    .cls-2 { fill: red; }
                                                   `}
                                                        </style>

                                                    </defs>

                                                    <title />

                                                    <g id="fill">

                                                        <path className="cls-1" d="M27.35,8.49h-5s0-.08,0-.12a6.37,6.37,0,0,0-12.74,0s0,.08,0,.12h-5a1,1,0,1,0,0,1.93h2.7V26.14A3.86,3.86,0,0,0,11.21,30h9.58a3.86,3.86,0,0,0,3.86-3.86V10.42h2.7a1,1,0,1,0,0-1.93ZM11.56,8.37a4.44,4.44,0,0,1,8.88,0s0,.08,0,.12H11.54S11.56,8.41,11.56,8.37Z" />

                                                        <path className="cls-2" d="M12.76,25a1,1,0,0,1-1-1V15.4a1,1,0,0,1,1.93,0v8.65A1,1,0,0,1,12.76,25Z" />

                                                        <path className="cls-2" d="M19.24,25a1,1,0,0,1-1-1V15.4a1,1,0,0,1,1.93,0v8.65A1,1,0,0,1,19.24,25Z" />

                                                    </g>

                                                </svg>
                                            </span>
                                            <span onClick={() => handleclick(skillsGroup.id)} className="ml-4 text-lg text-danger cursor-pointer active:opacity-50">
                                                <svg width="30px" height="30px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                                    <path d="M20.2071 3.79288C18.9882 2.57392 17.0119 2.57392 15.7929 3.79288L8.68463 10.9012C8.30015 11.2856 8.0274 11.7674 7.89552 12.2949L7.02988 15.7574C6.94468 16.0982 7.04453 16.4587 7.29291 16.7071C7.54129 16.9555 7.90178 17.0553 8.24256 16.9701L11.7051 16.1045C12.2326 15.9726 12.7144 15.6999 13.0988 15.3154L20.2071 8.20709C21.4261 6.98813 21.4261 5.01183 20.2071 3.79288Z" fill="green" />
                                                    <path fillRule="evenodd" clipRule="evenodd" d="M2 7C2 4.23858 4.23858 2 7 2H12C12.5523 2 13 2.44772 13 3C13 3.55228 12.5523 4 12 4H7C5.34315 4 4 5.34315 4 7V17C4 18.6569 5.34315 20 7 20H17C18.6569 20 20 18.6569 20 17V12C20 11.4477 20.4477 11 21 11C21.5523 11 22 11.4477 22 12V17C22 19.7614 19.7614 22 17 22H7C4.23858 22 2 19.7614 2 17V7Z" fill="green" />
                                                </svg>
                                            </span>
                                        </td>
                                    </motion.tr>
                                ))}
                            </tbody>
                        </table>
                    </section>
                    <section>
                        {
                            isModal && (
                                <div className=" flex justify-center items-center overflow-y-auto overflow-x-hidden fixed top-0 right-0 left-0 z-50 justify-center items-center w-full md:inset-0 h-[calc(100%-1rem)] max-h-full">
                                    <div className="relative p-4 w-full max-w-2xl max-h-full">
                                        {/* <!-- Modal content --> */}
                                        <div className="relative bg-white rounded-lg shadow dark:bg-gray-700">
                                            {/* <!-- Modal header --> */}
                                            <div className="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600">
                                                <h3 className="text-xl font-semibold text-gray-900 dark:text-white">
                                                    Modifier un groupe de compétence
                                                </h3>
                                                <button onClick={() => setIsModal(false)} type="button" className="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white" data-modal-hide="static-modal">
                                                    <svg className="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
                                                        <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
                                                    </svg>
                                                </button>
                                            </div>
                                            {/* <!-- Modal body --> */}
                                            <form className="px-5 py-5" onSubmit={(e) => handleUpdate(e,selectedId)}>
                                                <div>
                                                    <div>
                                                        <label htmlFor="lastname">Titre</label>
                                                        <input
                                                            type="text"
                                                            name="title"
                                                            id="title"
                                                            value={newTitle}
                                                            onChange={handleNewTitleChange}
                                                            className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                                        />
                                                    </div>
                                                </div>
                                                <button className="bg-blue-500 text-white px-4 py-2 rounded-md mt-4">
                                                    Terminé
                                                </button>
                                            </form>
                                        </div>
                                    </div>
                                </div>
                            )
                        }



                    </section>
                </div>
            </section>
        </div>
    );
}
