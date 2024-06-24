"use client";
import { useState } from 'react'
import axios from 'axios'
import SideBar from '../sidebar/page';

function AddUser() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [confirmPassword, setConfirmPassword] = useState('')
    const [error, setError] = useState('')
    const [isValidEmail, setIsValidEmail] = useState(false)
    const handleEmailChange = (e: any) => {
        const inputEmail = e.target.value
        const regex = /^[a-zA-Z0-9._%+-]+@gmail\.com$/

        setEmail(inputEmail)
        setIsValidEmail(regex.test(inputEmail))
    }

    async function handleAddUser() {
        if (email.trim() === '' || password.trim() === '') {
            setError('*Veuillez remplir tout les champs')

            return;
        }
        if (password !== confirmPassword) {
            setError('Les mots de passe ne correspondent pas')
            return;
        }
        try {
            const { data } = await axios.post(`http://localhost:5236/api/users`, {
                email: email.trim(),
                password: password.trim()
            }, {
                withCredentials: true
            });

            console.log(data);

            window.location.href = '/services';

        } catch (error) {
            setError(`Email et/ou mot de passe incorrect(s)!`);
        }
    }

    return (
        <div>
            <SideBar />
            <section className="bg-gray-50 dark:bg-gray-900">
                <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">

                    <div className="w-full bg-white rounded-lg shadow dark:border md:mt-0 sm:max-w-md xl:p-0 dark:bg-gray-800 dark:border-gray-700">
                        <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
                            <h1 className="text-center font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                                Ajout d'un nouvel utilisateur
                            </h1>
                            <div className="space-y-4 md:space-y-6">
                                <div>
                                    <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white" htmlFor="LoginEmail">Email</label>
                                    <input value={email}
                                        onChange={handleEmailChange} type="email" id="LoginEmail" className={` form-control ${isValidEmail ? 'is-valid' : ''} ${!isValidEmail && email ? 'is-invalid' : ''} bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="email@gmail.com"`} />
                                    {!isValidEmail && email && <div className="text-red-700 invalid-feedback">Veuillez saisir une adresse e-mail valide se terminant par @gmail.com</div>}
                                </div>
                                <div>
                                    <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white" htmlFor="LoginPassword">Password</label>
                                    <input value={password}
                                        onChange={(e) => setPassword(e.target.value)} type="password" name="password" id="LoginPassword" className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" />
                                </div>
                                <div className="flex items-center justify-between">

                                </div>
                                <div>
                                    <label className="block mb-2 text-sm font-medium text-gray-900 dark:text-white" htmlFor="ConfirmPassword">Confirm Password</label>
                                    <input value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} type="password" name="confirmPassword" id="ConfirmPassword" className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" />
                                </div>
                                {error && <div className="text-red-700">{error}</div>}
                                <button onClick={handleAddUser} type="submit" className="w-full text-white bg-primary-600 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-primary-600 dark:hover:bg-primary-700 dark:focus:ring-primary-800">Ajouter</button>

                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    )
}

export default AddUser
