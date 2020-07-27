<template>
    <div id="test">
        <button v-on:click="testing">Test</button>
    </div>
</template>

<script>
    var test = new Vue({
        el: '#test',
        data: 'Testing'
        methods: {
            testing: function () {
                this.message = this.message.reverse()
            }
        }
    })
</script>
